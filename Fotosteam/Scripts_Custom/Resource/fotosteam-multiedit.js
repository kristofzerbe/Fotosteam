var MultiEditFields = {
    IsPrivate: 1,
    AllowFullSizeDownload: 2,
    AllowCommenting: 4,
    AllowRating: 8,
    AllowSharing: 16,
    IsForStoryOnly: 32,
    AllowPromoting: 64,
    Category: 128,
    Topics: 256,
    LocationId: 512,
    EventId: 1024,
    License: 2048
};
var MultiEditData = {
    "IsPrivate": false,
    "AllowFullSizeDownload": false,
    "AllowCommenting": false,
    "AllowRating": false,
    "AllowSharing": false,
    "IsForStoryOnly": false,
    "AllowPromoting": false,
    "Category": 0,
    "Topics": [],
    "LocationId": 0,
    "EventId": 0,
    "License": 0,
    "ToBeDeleted": false,
    "PhotoIds": [],
    "CubicleIds": [],
    "ReplaceExistingValues": true,
    "Fields": 0
};
// ---------------------------------------------
function setEdited(jField, flag) {
    jField.prev("h3").addClass("edited");
    $("#multiedit-save").show();
    MultiEditData.Fields |= flag;
    console.log(MultiEditData.Fields);
}
// ---------------------------------------------
function toggleEdited(e, flag) {
    var jE = $(e).parent("h3");
    if (jE.hasClass("edited")) {
        jE.removeClass("edited");
        MultiEditData.Fields -= flag;
    } else {
        jE.addClass("edited");
        $("#multiedit-save").show();
        MultiEditData.Fields |= flag;
    }
    console.log(MultiEditData.Fields);
}
// ---------------------------------------------
function triggerSelector(jE, jOverview) {
    var jCubicle = jE.parents(".cubicle"),
        jContainer = jCubicle.parents(".collection-container");

    jCubicle.toggleClass("selected");

    var iSelected = $(".cubicle.selected").length;

    if ( iSelected !== 0) {
        jContainer.addClass("selector");
        jContainer.find(".cubicle .cubicle-link").unbind().click(function (e) {
            e.preventDefault();
            triggerSelector($(this), jOverview);
        });

        if (iSelected === 1) {
            var data = {
                ChoosePlaceholder: i18n.get("choose-placeholder"),
                Categories: global_member.Categories,
                Topics: global_member.Topics,
                Locations: global_member.Locations,
                Events: global_member.Events
            };

            //Defaults setzen
            data.IsPrivateAttribute = (window.global_auth_member.Options.DefaultIsPrivate) ? "" : "checked";
            data.IsForStoryOnlyAttribute = "";
            data.ShowInOverviewAttribute = "";
            data.AllowFullSizeDownloadAttribute = (window.global_auth_member.Options.DefaultAllowFullSizeDownload) ? "checked" : "";
            data.AllowCommentingAttribute = (window.global_auth_member.Options.DefaultAllowCommenting) ? "checked" : "";
            data.AllowRatingAttribute = (window.global_auth_member.Options.DefaultAllowRating) ? "checked" : "";
            data.AllowSharingAttribute = (window.global_auth_member.Options.DefaultAllowSharing) ? "checked" : "";
            data.AllowPromotingAttribute = (window.global_auth_member.Options.DefaultAllowPromoting) ? "checked" : "";
            //---
            $.each(Licenses, function (index, value) {
                value.Selected = (value.Value === window.global_auth_member.Options.DefaultLicense);
            });
            data.Licenses = Licenses;

            var jMultiEdit = $(i18n.translate(fsTemplates.tMultiEditDialogBar(data)));

            jMultiEdit.find("#multiedit-delete").click(function () {
                popoutConfirm("delete-multiplefotos",
                    function () {
                        $(".cubicle.selected").each(function() {
                            var fname = $(this).prop("id"),
                                id = $(this).find("img").data("id");
                            MultiEditData.PhotoIds.push(id);
                            MultiEditData.CubicleIds.push(fname);
                        });
                        MultiEditData.ToBeDeleted = true;
                        executeSteamAction("multi-update", null, MultiEditData,
                            function (result) {
                                if (result.Data === true) {
                                    $.each(MultiEditData.CubicleIds, function(index, value) {
                                        $("div#" + value).remove();
                                    });
                                }

                                window.global_member.Fotos = window.global_member.Fotos.filter(function (foto) {
                                    return $.inArray(foto.Id, MultiEditData.PhotoIds) === -1;
                                });

                                var msg = i18n.get("delete-multiplefotos-affirm-message").replace("%1", MultiEditData.CubicleIds.length);
                                toastMessageText(msg);

                                jOverview.FillGridFotoGaps();
                                removeMultiEditor(jContainer);
                            },
                            function (result) { toastErrorText(result, "delete-multiplefotos-error"); }
                        );
                    }
                );
            });
            jMultiEdit.find("#multiedit-edit").click(function () {
                showMultiEditor();
            });
            jMultiEdit.find("#multiedit-cancel").click(function () {
                removeMultiEditor(jContainer);
            });
            showDialogBar(jMultiEdit);
            initMultiEditor(jContainer);
        }
        $("#multiedit-selected").html(i18n.get("count-selected").replace("%1", $(".cubicle.selected").length));

    } else {
        removeMultiEditor(jContainer);
    }

    return false;
}
// ---------------------------------------------
function initMultiEditor(jContainer) {
    
    $("head").append(
        '<style id="multi-editor-styles" type="text/css">' +
            '.yesno + .ios-ui-select.checked:before { content: "' + i18n.get("yes") + '";}' +
            '.yesno + .ios-ui-select:not(.checked):after { content: "' + i18n.get("no") + '";}' +
            '.pubpriv + .ios-ui-select.checked:before { content: "' + i18n.get("public") + '";}' +
            '.pubpriv + .ios-ui-select:not(.checked):after { content: "' + i18n.get("private") + '";}' +
            '.allowdeny + .ios-ui-select.checked:before { content: "' + i18n.get("allowed") + '";}' +
            '.allowdeny + .ios-ui-select:not(.checked):after { content: "' + i18n.get("denied") + '";}' +
        '</style>'
    );

    //$("#multieditor-selection .collection-wrapper").mCustomScrollbar({
    //    theme: "dark-thick",
    //    scrollInertia: 200
    //});

    $("#multieditor-controls").mCustomScrollbar({
        theme: "light",
        scrollInertia: 200,
        autoHideScrollbar: true
    });

    initMultiEditorControls();

    $("#multiedit-save").click(function () {
        storeMultiEdit(jContainer);
    });

    $("#multiedit-close").click(function () {
        hideMultiEditor();
    });
}
// ---
function initMultiEditorControls() {

    //iOS-Controls
    var jIsPrivate = $("#multieditor-controls #IsPrivate");
    jIsPrivate.iosCheckbox({
        change: function (e, checkValue) {
            MultiEditData.IsPrivate = !checkValue;
            setEdited($(e), MultiEditFields.IsPrivate);
        }
    });
    jIsPrivate.prev("h3").find("a").click(function (e) {
        e.preventDefault();
        MultiEditData.IsPrivate = jIsPrivate.prop('checked');
        toggleEdited(this, MultiEditFields.IsPrivate);
    });
    //---
    var jAllowFullSizeDownload = $("#multieditor-controls #AllowFullSizeDownload");
    jAllowFullSizeDownload.iosCheckbox({
        change: function (e, checkValue) {
            MultiEditData.AllowFullSizeDownload = checkValue;
            setEdited($(e), MultiEditFields.AllowFullSizeDownload);
        }
    });
    jAllowFullSizeDownload.prev("h3").find("a").click(function (e) {
        e.preventDefault();
        MultiEditData.AllowFullSizeDownload = jAllowFullSizeDownload.prop('checked');
        toggleEdited(this, MultiEditFields.AllowFullSizeDownload);
    });
    //---
    var jAllowCommenting = $("#multieditor-controls #AllowCommenting");
    jAllowCommenting.iosCheckbox({
        change: function (e, checkValue) {
            MultiEditData.AllowCommenting = checkValue;
            setEdited($(e), MultiEditFields.AllowCommenting);
        }
    });
    jAllowCommenting.prev("h3").find("a").click(function (e) {
        e.preventDefault();
        MultiEditData.AllowCommenting = jAllowCommenting.prop('checked');
        toggleEdited(this, MultiEditFields.AllowCommenting);
    });
    //---
    var jAllowRating = $("#multieditor-controls #AllowRating");
    jAllowRating.iosCheckbox({
        change: function (e, checkValue) {
            MultiEditData.AllowRating = checkValue;
            setEdited($(e), MultiEditFields.AllowRating);
        }
    });
    jAllowRating.prev("h3").find("a").click(function (e) {
        e.preventDefault();
        MultiEditData.AllowRating = jAllowRating.prop('checked');
        toggleEdited(this, MultiEditFields.AllowRating);
    });
    //---
    var jAllowSharing = $("#multieditor-controls #AllowSharing");
    jAllowSharing.iosCheckbox({
        change: function (e, checkValue) {
            MultiEditData.AllowSharing = checkValue;
            setEdited($(e), MultiEditFields.AllowSharing);
        }
    });
    jAllowSharing.prev("h3").find("a").click(function (e) {
        e.preventDefault();
        MultiEditData.AllowSharing = jAllowSharing.prop('checked');
        toggleEdited(this, MultiEditFields.AllowSharing);
    });
    //---
    var jIsForStoryOnly = $("#multieditor-controls #IsForStoryOnly");
    jIsForStoryOnly.iosCheckbox({
        change: function (e, checkValue) {
            MultiEditData.IsForStoryOnly = checkValue;
            setEdited($(e), MultiEditFields.IsForStoryOnly);
        }
    });
    jIsForStoryOnly.prev("h3").find("a").click(function (e) {
        e.preventDefault();
        MultiEditData.IsForStoryOnly = jIsForStoryOnly.prop('checked');
        toggleEdited(this, MultiEditFields.IsForStoryOnly);
    });
    //---
    var jAllowPromoting = $("#multieditor-controls #AllowPromoting");
    jAllowPromoting.iosCheckbox({
        change: function (e, checkValue) {
            MultiEditData.AllowPromoting = checkValue;
            setEdited($(e), MultiEditFields.AllowPromoting);
        }
    });
    jAllowPromoting.prev("h3").find("a").click(function (e) {
        e.preventDefault();
        MultiEditData.AllowPromoting = jAllowPromoting.prop('checked');
        toggleEdited(this, MultiEditFields.AllowPromoting);
    });

    //--- Chosen-Controls
    var chosenOptions = {
        no_results_text: i18n.get("term-not-found"),
        width: "100%"
    };

    var jCategories = $("#multieditor-controls #Categories");
    jCategories.chosen(chosenOptions)
        .on('change', function (event, params) {
            var newValue = 0;
            $(this).find("option:selected").each(function () {
                newValue += parseInt($(this).val());
            });
            MultiEditData.Category = newValue;
            setEdited($(this), MultiEditFields.Category);
        }
    );
    jCategories.prev("h3").find("a").click(function (e) {
        e.preventDefault();
        var newValue = 0;
        jCategories.find("option:selected").each(function () {
            newValue += parseInt($(this).val());
        });
        MultiEditData.Category = newValue;
        toggleEdited(this, MultiEditFields.Category);
    });
    //---
    var jTopics = $("#multieditor-controls #Topics");
    jTopics.chosen(chosenOptions)
        .on('change', function (event, params) {
            if (params.selected) {
                MultiEditData.Topics.push(params.selected);
            } else if (params.deselected) {
                MultiEditData.Topics.remove(params.deselected);
            }
            setEdited($(this), MultiEditFields.Topics);
        }
    );
    jTopics.prev("h3").find("a").click(function (e) {
        e.preventDefault();
        MultiEditData.Topics = jTopics.chosen().val();
        toggleEdited(this, MultiEditFields.Topics);
    });
    //---
    var jLocation = $("#multieditor-controls #Location");
    jLocation.chosen(chosenOptions)
        .on('change', function (event, params) {
            MultiEditData.LocationId = params.selected;
            setEdited($(this), MultiEditFields.LocationId);
        }
    );
    jLocation.prev("h3").find("a").click(function (e) {
        e.preventDefault();
        MultiEditData.LocationId = jLocation.chosen().val();
        toggleEdited(this, MultiEditFields.LocationId);
    });
    //---
    var jEvent = $("#multieditor-controls #Event");
    jEvent.chosen(chosenOptions)
        .on('change', function (event, params) {
            MultiEditData.EventId = params.selected;
            setEdited($(this), MultiEditFields.EventId);
        }
    );
    jEvent.prev("h3").find("a").click(function (e) {
        e.preventDefault();
        MultiEditData.EventId = jEvent.chosen().val();
        toggleEdited(this, MultiEditFields.EventId);
    });
    //---
    var jLicense = $("#multieditor-controls #License");
    jLicense.chosen(chosenOptions)
        .on('change', function (event, params) {
            MultiEditData.License = params.selected;
            setEdited($(this), MultiEditFields.License);
        }
    );
    jLicense.prev("h3").find("a").click(function (e) {
        e.preventDefault();
        MultiEditData.License = jLicense.chosen().val();
        toggleEdited(this, MultiEditFields.License);
    });
}
// ---------------------------------------------
function removeMultiEditor(jContainer) {
    hideMultiEditor();
    MultiEditData.PhotoIds = [];
    MultiEditData.CubicleIds = [];
    jContainer.removeClass("selector");
    jContainer.find(".cubicle.selected").each(function () {
        var cub = $(this);
        cub.removeClass("selected");
    });
    doFullsizable($(".cubicle-link"), {
        detailButton: true
    });
    removeDialogBar();
    $("#multi-editor-styles").remove();
}
// ---------------------------------------------
function showMultiEditor() {
    
    $(".cubicle.selected").each(function () {
        var fname = $(this).prop("id"),
            id = $(this).find("img").data("id"),
            foto = Enumerable
                .From(window.global_member.Fotos)
                .Where(function (x) { return x.Name == fname; })
                .Select()
                .FirstOrDefault(),
            data = {
                Foto: foto
            },
            jFoto = $(i18n.translate(fsTemplates.tCubicle(data)));

        $("#multieditor-selection .collection-wrapper").append(jFoto);
        MultiEditData.PhotoIds.push(id);
        MultiEditData.CubicleIds.push(fname);
    });

    $("html").addClass("dim");

    $("#multiedit-edit").fadeOut();
    $("#multiedit-delete").fadeOut();
    $("#multiedit-close").fadeIn();

    if (MultiEditData.Fields !== 0) {
        $("#multiedit-save").fadeIn();
    }

    $("#multieditor").fadeIn(1000, function () {
        $("#multieditor").css("display", ""); /* irgendein Sch** setzt mein zweiten Anzeigen 'display:flex' ein !?*/
        fitEditor();
        $(window).on('resize', function () { fitEditor(); });
    });

    $("#dialogbar").css({ height: $(window).innerHeight() });

    function fitEditor() {
        $("#multieditor").css({
            height: $("#dialogbar").height() - $("#multieditor").position().top
        });
        if (global_format === "S") {
            var cubCount = $("#multieditor-selection .cubicle").length,
                cubRows = Math.ceil(cubCount / 4),
                cubH = $("#multieditor-selection .cubicle").height();

            if (cubH < 81) {
                cubRows = (cubRows < 3) ? cubRows : 3;
            } else {
                cubRows = (cubRows < 2) ? cubRows : 2;
            }
            $("#multieditor-selection").css({ height: cubRows * cubH });
            $("#multieditor-controls").css({
                height: $("#dialogbar").height() - $("#multieditor-controls").position().top
            });
        } else {
            $("#multieditor-selection").css("height", "100%");
            $("#multieditor-controls").css("height", "100%");
        }

        $("#multieditor-controls").mCustomScrollbar({
            theme: "light",
            scrollInertia: 200,
            autoHideScrollbar: true
        });

    }
}
// ---------------------------------------------
function hideMultiEditor() {
    $("html").removeClass("dim");
    $("#multieditor").fadeOut(500, function() {
        $("#multieditor").css("height", "");
    });
    $("#dialogbar").css({ height: $("#dialogbar").data("init-height") });
    $("#multiedit-edit").fadeIn();
    $("#multiedit-delete").fadeIn();
    $("#multiedit-close").fadeOut();
    $("#multiedit-save").fadeOut();
    setTimeout(function () {
        $("#multieditor-selection .collection-wrapper").empty();
        MultiEditData.PhotoIds.length = 0;
        MultiEditData.CubicleIds.length = 0;        
    }, 500);
}
// ---------------------------------------------
function storeMultiEdit(jContainer) {

    var count = $(".cubicle.selected").length;

    executeSteamAction("multi-update", null, MultiEditData,
        function(result) {
            if (result.Data === true) {

                $.each(MultiEditData.PhotoIds, function (index, value) {
                    if (MultiEditData.Fields & MultiEditFields.IsPrivate === MultiEditFields.IsPrivate) {
                        updateLocalFoto(value, "IsPrivate", MultiEditData.IsPrivate);                        
                    }
                    if (MultiEditData.Fields & MultiEditFields.AllowFullSizeDownload === MultiEditFields.AllowFullSizeDownload) {
                        updateLocalFoto(value, "AllowFullSizeDownload", MultiEditData.AllowFullSizeDownload);
                    }
                    if (MultiEditData.Fields & MultiEditFields.AllowCommenting === MultiEditFields.AllowCommenting) {
                        updateLocalFoto(value, "AllowCommenting", MultiEditData.AllowCommenting);
                    }
                    if (MultiEditData.Fields & MultiEditFields.AllowRating === MultiEditFields.AllowRating) {
                        updateLocalFoto(value, "AllowRating", MultiEditData.AllowRating);
                    }
                    if (MultiEditData.Fields & MultiEditFields.AllowSharing === MultiEditFields.AllowSharing) {
                        updateLocalFoto(value, "AllowSharing", MultiEditData.AllowSharing);
                    }
                    if (MultiEditData.Fields & MultiEditFields.IsForStoryOnly === MultiEditFields.IsForStoryOnly) {
                        updateLocalFoto(value, "IsForStoryOnly", MultiEditData.IsForStoryOnly);
                    }
                    if (MultiEditData.Fields & MultiEditFields.AllowPromoting === MultiEditFields.AllowPromoting) {
                        updateLocalFoto(value, "AllowPromoting", MultiEditData.AllowPromoting);
                    }
                    if (MultiEditData.Fields & MultiEditFields.Category === MultiEditFields.Category) {
                        updateLocalFoto(value, "Category", MultiEditData.Category);
                    }
                    if (MultiEditData.Fields & MultiEditFields.Topics === MultiEditFields.Topics) {
                        updateLocalFoto(value, "Topics_ResetWithIdList", MultiEditData.Topics);
                    }
                    if (MultiEditData.Fields & MultiEditFields.LocationId === MultiEditFields.LocationId) {
                        updateLocalFoto(value, "LocationId", MultiEditData.LocationId);
                    }
                    if (MultiEditData.Fields & MultiEditFields.EventId === MultiEditFields.EventId) {
                        updateLocalFoto(value, "EventId", MultiEditData.EventId);
                    }
                    if (MultiEditData.Fields & MultiEditFields.License === MultiEditFields.License) {
                        updateLocalFoto(value, "License", MultiEditData.License);
                    }
                });

                removeMultiEditor(jContainer);

                var msg = i18n.get("multiupdate-message").replace("%1", count);
                toastMessageText(msg);
            }
        },
        function (result) { toastErrorText(result, "multiupdate-error"); }
    );
}