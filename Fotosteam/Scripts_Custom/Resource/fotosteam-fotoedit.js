function initFotoEdit(data) {

    $("head").append(
        '<style type="text/css">' +
            '.yesno + .ios-ui-select.checked:before { content: "' + i18n.get("yes") + '";}' +
            '.yesno + .ios-ui-select:not(.checked):after { content: "' + i18n.get("no") + '";}' +
            '.pubpriv + .ios-ui-select.checked:before { content: "' + i18n.get("public") + '";}' +
            '.pubpriv + .ios-ui-select:not(.checked):after { content: "' + i18n.get("private") + '";}' +
            '.allowdeny + .ios-ui-select.checked:before { content: "' + i18n.get("allowed") + '";}' +
            '.allowdeny + .ios-ui-select:not(.checked):after { content: "' + i18n.get("denied") + '";}' +
        '</style>'
    );

    //DATA
    data.IsPrivateAttribute = (data.Foto.IsPrivate) ? "" : "checked";
    data.IsForStoryOnlyAttribute = (data.Foto.IsForStoryOnly) ? "checked" : "";
    data.ShowInOverviewAttribute = (data.Foto.ShowInOverview) ? "checked" : "";
    data.AllowFullSizeDownloadAttribute = (data.Foto.AllowFullSizeDownload) ? "checked" : "";
    data.AllowCommentingAttribute = (data.Foto.AllowCommenting) ? "checked" : "";
    data.AllowRatingAttribute = (data.Foto.AllowRating) ? "checked" : "";
    data.AllowSharingAttribute = (data.Foto.AllowSharing) ? "checked" : "";
    data.AllowPromotingAttribute = (data.Foto.AllowPromoting) ? "checked" : "";    
    //--
    $.each(Licenses, function (index, value) {
        value.Selected = (value.Value === data.Foto.LicenseValue);
    });
    data.Licenses = Licenses;
    //--
    data.ChoosePlaceholder = i18n.get("choose-placeholder");
    //--
    data.Categories = Enumerable
        .From(data.Categories)
        .Where(function (x) { return x.CategoryType !== "notset"; })
        .OrderBy(function (x) { return x.CategoryName; }).ToArray();
    $.each(data.Categories, function (index, value) {
        value.Selected = Enumerable.From(data.Foto.CategoryList).Where(function (x) { return x.CategoryName === value.CategoryName; }).Count() > 0;
    });
    //--
    data.Topics = Enumerable.From(data.Topics).OrderBy(function (x) { return x.TopicName; }).ToArray();
    $.each(data.Topics, function (index, value) {
        value.Selected = Enumerable.From(data.Foto.TopicList).Where(function (x) { return x.TopicId === value.TopicId; }).Count() > 0;
    });
    //--
    data.Locations = Enumerable
        .From(data.Locations)
        .Where(function (x) { return x.LocationId !== data.AuthMember.HomeLocation.LocationId; })
        .OrderBy(function (x) { return x.LocationName; }).ToArray();
    $.each(data.Locations, function (index, value) {
        value.Selected = (data.Foto.Location !== undefined && value.LocationId === data.Foto.Location.LocationId);
    });
    //--
    data.Events = Enumerable
        .From(data.Events)
        .OrderBy(function (x) { return x.LocationName; }).ToArray();
    $.each(data.Events, function (index, value) {
        value.Selected = (data.Foto.Event !== undefined && value.EventId === data.Foto.Event.EventId);
    });

    var jFoto = $(i18n.translate(fsTemplates.tFotoEdit(data)));

    jFoto.find('#foto-menu').dropit();

    //PRIVATE
    jFoto.find("#IsPrivate").iosCheckbox({
        change: function (e, checkValue) {
            updateFoto(data.Foto.Id, "IsPrivate", !checkValue, "status");
        }
    });

    //DOWNLOAD
    jFoto.find("#AllowFullSizeDownload").iosCheckbox({
        change: function (e, checkValue) {
            updateFoto(data.Foto.Id, "AllowFullSizeDownload", checkValue, "download-original");
        }
    });

    //COMMENTING
    jFoto.find("#AllowCommenting").iosCheckbox({
        change: function (e, checkValue) {
            updateFoto(data.Foto.Id, "AllowCommenting", checkValue, "foto-commenting");
        }
    });

    //RATING
    jFoto.find("#AllowRating").iosCheckbox({
        change: function (e, checkValue) {
            updateFoto(data.Foto.Id, "AllowRating", checkValue, "foto-rating");
        }
    });

    //SHARING
    jFoto.find("#AllowSharing").iosCheckbox({
        change: function (e, checkValue) {
            updateFoto(data.Foto.Id, "AllowSharing", checkValue, "foto-sharing");
        }
    });

    //FORSTORYONLY
    jFoto.find("#IsForStoryOnly").iosCheckbox({
        change: function (e, checkValue) {
            updateFoto(data.Foto.Id, "IsForStoryOnly", checkValue, "story-only");
        }
    });
    jFoto.find("#ShowInOverview").iosCheckbox({
        change: function (e, checkValue) {
            updateFoto(data.Foto.Id, "ShowInOverview", checkValue, "show-in-overview");
        }
    });
    
    //PROMOTE
    jFoto.find("#AllowPromoting").iosCheckbox({
        change: function (e, checkValue) {
            updateFoto(data.Foto.Id, "AllowPromoting", checkValue, "fotosteam-promote");
        }
    });

    //TITLE
    jFoto.find("#Title").change(function () {
        updateFoto(data.Foto.Id, "Title", $(this).val(), "title");
        $("header h2.title").html($(this).val());
    });

    //DESCRIPTION
    var sDescription;
    jFoto.find("#Description").MarkdownDeep({
        disableTabHandling: true,
        resizebar: false,
        toolbar: true
    });
    jFoto.find(".mdd_links").append('<a href="#" class="mdd_button" id="mdd_save" title="' + i18n.get("save") + '" tabindex="-1"></a>');
    jFoto.find("#mdd_save").click(function (e) {
        e.preventDefault();
        updateFoto(data.Foto.Id, "Description", jFoto.find("#Description").val(), "description");
    });
    setupMDDToolbar(jFoto.find(".mdd_toolbar"));

    //COLOR
    jFoto.find("#color-reset").click(function () {
        resetFotoColor(data.Foto.Id); return false;
    });
    //--
    jFoto.find("#Color").change(function () {
        var colorHex = $(this).val(),
            colorR = hexToR(colorHex),
            colorG = hexToG(colorHex),
            colorB = hexToB(colorHex);
        updateFoto(data.Foto.Id, "Color", (colorR + "," + colorG + "," + colorB), "foto-color");
        $("#color-label").text(colorHex);
        $("body").css("background-color", colorHex);
    });

    var chosenOptions = {
        no_results_text: i18n.get("term-not-found"),
        width: "100%"
    };

    //CATEGORIES
    jFoto.find("#Categories").chosen(chosenOptions)
        .on('change', function (event, params) {
            var newValue = 0;
            $(this).find("option:selected").each(function () {
                newValue += parseInt($(this).val());
            });
            updateFoto(data.Foto.Id, "Category", newValue, "categories");
        }
    );

    //TOPICS
    jFoto.find("#Topics").chosen(chosenOptions)
        .on('change', function (event, params) {
            var model = { PhotoId: data.Foto.Id };
            if (params.selected) {
                model.Action = "add"; model.TopicId = params.selected;
            } else if (params.deselected) {
                model.Action = "remove"; model.TopicId = params.deselected;
            }
            if (model.TopicId !== 0) {
                executeSteamAction("edit-phototopic", null, model,
                    function () {
                        toastMessageText(i18n.get("phototopic-message-" + model.Action));
                        updateLocalFoto(data.Foto.Id, "Topics", (model.Action === "add" ? model.TopicId : -model.TopicId), "topics");
                    },
                    function (result) { toastErrorText(result, "foto-reset-color-error"); }
               );
            }
        }
    );

    //LOCATION
    jFoto.find("#Location").chosen(chosenOptions)
        .on('change', function (event, params) {
            updateFoto(data.Foto.Id, "LocationId", params.selected, "location");

            var location = Enumerable
                .From(data.Locations)
                .Where(function (x) { return x.LocationId === parseInt(params.selected); })
                .FirstOrDefault();

            if (location) {
                loadMap(
                    jFoto.find(".location-map").show(),
                    location.LocationLatitude,
                    location.LocationLongitude,
                    false);
            }
        }
    );

    //EVENT
    jFoto.find("#Event").chosen(chosenOptions)
        .on('change', function (event, params) {
            updateFoto(data.Foto.Id, "EventId", params.selected, "event");
        }
    );

    //LICENSES
    jFoto.find("#License")
        //.on('chosen:ready', function (chosen) {
        //TODO: Icons in Listeneintrag anzeigen
        //$(chosen.target).next("div.chosen-container ul.chosen-results li").each(function () {

        //    var licValue = 33,
        //        licArray = getLicenseArray(licValue),
        //        licName = getLicenseName(licArray[0], licArray[1]),
        //        jLicIcons = $('<span class="icons-right"></span>');

        //    $.each(licArray[0], function (index, value) {
        //        jLicIcons.append('<i class="icon icon-' + value + '"></i>');
        //    });

        //    $(this).append(jLicIcons).prop("title", licName);
        //});
    //})
    .chosen(chosenOptions)
        .on('change', function (event, params) {
            updateFoto(data.Foto.Id, "License", params.selected, "license");
        }
    );
    //.trigger("chosen:activate").trigger("chosen:ready");


    //FOCAL-SELECTION
    var jImg = jFoto.find("#foto-meta-image"),
        jWrapper = jFoto.find("#focalpoint-selection");
    //--
    jFoto.find("#activate-focalpoint-selection").unbind().click(function () {
        showFocalPointSelector(jImg, jWrapper);
    });
    //--
    jFoto.find("#cancel-focalpoint-selection").unbind().click(function () {
        hideFocalPointSelector(jImg, jWrapper);
    });
    //--
    jFoto.find("#save-focalpoint-selection").unbind().click(function () {
        storeFocalPoint(jImg, jWrapper);
    });

    //Resize-Binding
    $(window).on('resize', function () {
        hideFocalPointSelector(jImg, jWrapper);
    });
    hideFocalPointSelector(jImg, jWrapper);

    return jFoto;
}
// ---------------------------------------------
function resetFotoColor(id) {
    $("body").toggleClass("wait");
    executeSteamAction("color-reset", null, id,
        function (result) {
            var arrColor = result.Data.split(","),
                colorR = arrColor[0],
                colorG = arrColor[1],
                colorB = arrColor[1],
                colorHex = "#" + rgbToHex(colorR, colorG, colorB);

            $("#Color").val(colorHex);
            $("#color-label").text(colorHex);
            $("body").css("background-color", "rgb(" + result.Data + ")");
            toastMessageText(i18n.get("foto-reset-color-message"));
            $("body").toggleClass("wait");
        },
        function (result) {
            toastErrorText(result, "foto-reset-color-error");
            $("body").toggleClass("wait");
        }
    );
}
// ---------------------------------------------
function deleteFoto(id) {
    popoutConfirm("foto-delete",
        function () {
            executeSteamAction("delete-photo", { Id: id }, null,
                function () {
                    history.go(-1);
                },
                function (result) { toastErrorText(result, "foto-delete-error"); }
            );
        }
    );

}
// ---------------------------------------------
function UploadNewFotoVersion(id) {
    $('#upload-new-version').change(function () {

        toastNotice("foto-upload-new-version-wait");

        var file = this.files[0];

        if (file.type !== "image/jpeg") {
            toastError("foto-upload-new-version-filetype-error-message", "foto-upload-new-version-filetype-error-title");
            return;
        }

        var formData = new FormData($("#upload-new-version-form")[0]);

        $.ajax({
            type: "POST",
            url: "/api/data/updatephoto/" + id,
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (result) {
                toastHideAll();

                var newFoto = new Foto(result.Data, global_auth_member),
                    d = new Date();
                newFoto.SetPageStyles(d.getTime());
                $("#foto-meta-image").attr("src", newFoto.Url640 + "?" + d.getTime());
                replaceFoto(id, newFoto);

                toastMessage("foto-upload-new-version-message");
            },
            error: function (result) {
                toastErrorText(result, "foto-upload-new-version-error");
            }
        });
    });
    $('#upload-new-version').trigger('click');
}
// ---------------------------------------------
var focalPointSelector;
function showFocalPointSelector(jImg, jWrapper) {

    var img = new Image();

    img.src = jImg.attr("src");

    img.onload = function () {
        hideFocalPointSelector(jImg, jWrapper);

        // Start with a tiny area in the center
        var selLength = 0;
        var x1;
        var y1;
        var x2;
        var y2;

        var imageHeight = jImg.height();
        var imageWidth = jImg.width();

        if (imageWidth > imageHeight) {
            selLength = imageHeight;
            x1 = jImg.data("left") * imageWidth;
            x2 = x1 + selLength;
            y1 = 0;
            y2 = imageHeight;
        } else {
            selLength = imageWidth;
            x1 = 0;
            x2 = selLength;
            y1 = jImg.data("top") * imageHeight;
            y2 = y1 + selLength;
        }

        focalPointSelector = jImg.imgAreaSelect({
            fadeSpeed: 200,
            handles: false,
            instance: true,
            AspectRatio: "1:1",
            resizable: false,
            show: true,
            movable: true,
            minHeight: selLength,
            minWidth: selLength
        });

        focalPointSelector.setSelection(x1, y1, x2, y2);
        focalPointSelector.update();

        jWrapper.find("#activate-focalpoint-selection").hide();
        jWrapper.find("#focalpoint-selection-info").hide();

        jWrapper.find("#save-focalpoint-selection").show();
        jWrapper.find("#cancel-focalpoint-selection").show();

        $.scrollTo(jImg, 500, { offset: -60 });
    };
}
// ---------------------------------------------
function hideFocalPointSelector(jImg, jWrapper) {
    if (focalPointSelector) {
        focalPointSelector.remove();
        jImg.imgAreaSelect({ remove: true });
        $(".imgareaselect-selection").parent().remove();
        $(".imgareaselect-outer").remove();
        focalPointSelector = null;

        jWrapper.find("#save-focalpoint-selection").hide();
        jWrapper.find("#cancel-focalpoint-selection").hide();

        jWrapper.find("#activate-focalpoint-selection").prop("disabled", false).show();
        jWrapper.find("#focalpoint-selection-info").prop("disabled", false).show();
    }
}
// ---------------------------------------------
function storeFocalPoint(jImg, jWrapper) {

    toastNotice("foto-set-focalpoint-wait");
    jWrapper.find("#save-focalpoint-selection").prop("disabled", true);
    jWrapper.find("#cancel-focalpoint-selection").prop("disabled", true);

    var id = jImg.data("id"),
        selection = focalPointSelector.getSelection(),
        imageHeight = jImg.height(),
        imageWidth = jImg.width(),
        x = selection.x1 / imageWidth,
        y = selection.y1 / imageHeight;

    var model = {
            PhotoId: id,
            XPercentage: x,
            YPercentage: y
        };

    $.ajax({
        type: "PUT",
        url: "/api/data/thumbs",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(model),
        success: function (result) {
            toastHideAll();
            hideFocalPointSelector(jImg, jWrapper);
            toastMessage("foto-set-focalpoint-message");
        },
        error: function (result) {
            hideFocalPointSelector(jImg, jWrapper);
            toastErrorText(result, "foto-set-focalpoint-error");            
        }
    });
}


