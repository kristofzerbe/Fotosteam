// ---------------------------------------------
function initDashboard(member, routeHash) {

    member.SocialMediaFullList = SocialMediaFullList(member.SocialMedias);

    //(Location-Liste OHNE HomeLocation!)
    member.LocationList = Enumerable
        .From(member.Locations)
        .Where(function (x) { return x.LocationId !== member.HomeLocation.LocationId; })
        .OrderBy(function (x) { return x.LocationName; }).ToArray();

    //OPTIONS, (bool > Attributes)
    var optionAttributes = {};
    for (var property in member.Options) {
        if (member.Options[property] === true || member.Options[property] === false) {
            if (property === "DefaultIsPrivate") {
                optionAttributes[property] = (member.Options[property]) ? "" : "checked";
            } else {
                optionAttributes[property] = (!member.Options[property]) ? "" : "checked";
            }
        }
    }
    $.each(Licenses, function (index, value) {
        value.Selected = (value.Value === member.Options.DefaultLicense);
    });

    var tdata = {
            Member: member,
            UsesDropbox: (member.StorageAccessType === 1),
            OptionAttributes: optionAttributes,
            OptionLanguages: [
                { Iso: "de", Caption: i18n.get("language-de"), Selected: (member.Options.Language === 0) },
                { Iso: "en", Caption: i18n.get("language-en"), Selected: (member.Options.Language === 1) }
            ],
            OptionLicenses: Licenses,
            ChoosePlaceholder: i18n.get("choose-placeholder"),
            HasWebHook: (member.StorageProviderKey === "dropbox") ? true : false,
            UsesWebHook: member.Options.UseDropboxWebhook,
            OverwriteExistingPhoto: member.Options.OverwriteExistingPhoto
        },
        sDashboard = i18n.translate(fsTemplates.tDashboard(tdata)),
        jDashboard = $(sDashboard);

    //OPTIONS
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
    //--
    jDashboard.find(".options .ios").iosCheckbox({
        change: function(e, checkValue) {
            var eOption = $(e);
            if ($(e).attr("id") === "DefaultIsPrivate") { checkValue = !checkValue; }
            updateModel("MemberOption", member.Id,
                eOption.attr("id"),
                checkValue,
                eOption.prev("label").data("i18n"),
                function () {}
            );
        }
    });

    //--
    var chosenOptions = {
        no_results_text: i18n.get("term-not-found"),
        width: "100%",
        disable_search: true
    };
    //--
    jDashboard.find(".options select").chosen(chosenOptions)
        .on('change', function (event, params) {
            var eOption = $(this);
            updateModel("MemberOption", member.Id,
                eOption.attr("id"),
                params.selected,
                eOption.prev("label").data("i18n"),
                function () {
                    if (eOption.attr("id") === "Language") { changeLanguage(params.selected); }
                }
            );
        }
    );

    //PROFILE-MAP
    var jMap = jDashboard.find("#profile-map"),
        jMapAddress = jDashboard.find("#profile-map-address"),
        jRemove = jDashboard.find("#profile-map-remove"),
        jSave = jDashboard.find("#profile-map-save");

    var $map = new editProfileMap(jMap,
        member.HomeLocation.LocationLatitude,
        member.HomeLocation.LocationLongitude,
        jMapAddress,
        member,
        function (location) {
            //wenn Location-Id 0 > POST eines neuen Location-Objekts
            //ansonsten PUT des Location-Objekts mit Nachfrage, 
            //ob ein neues angelegt werden soll, um zu verhindern, 
            //dass sich die Location von Bildern verändert
            jSave.prop("disabled", false).removeClass("ghost").unbind().click(function () {
                var fShow = function () {
                    jSave.prop("disabled", true).addClass("ghost");
                    jRemove.prop("disabled", false).removeClass("ghost");
                };
                if (member.HomeLocation.LocationId === 0) {
                    member.storeHomeLocation(location, false, fShow);
                } else {
                    popoutConfirm("overwrite-homelocation",
                        function () { member.storeHomeLocation(location, true, fShow); },
                        function () { member.storeHomeLocation(location, false, fShow); });
                }

            });
        }
    );
    //--
    jMapAddress.change(function () {
        $map.setAddress($(this).val());
    });
    //--
    jSave.prop("disabled", true).addClass("ghost");
    if (member.HomeLocation.LocationId === 0) jRemove.prop("disabled", true).addClass("ghost");
    jRemove.click(function () {
        popoutConfirm("remove-homelocation",
            function () {
                member.removeHomeLocation(function () {
                    jRemove.prop("disabled", true).addClass("ghost");
                    window.location.reload(); //TODO:Reset Marker to 0/0
                });
            });
    });

    //APPLY CONTENT
    $("#content").empty().append(jDashboard);

    //TABS
    var tabOptions = {
        baseUrl: "/dashboard",
        firstTab: (routeHash) ? routeHash : "profile"
    };
    $('#dashboard-tabs').tabulous(tabOptions);

    //INPUTS
    $(".input input[type='text']").Inputs();
    $(".input input[type='email']").Inputs();
    $(".input textarea").Inputs();

    
    $("#profile-common-form").validate({
        rules: {
            PlainName: {
                required: true
            },
            EMail: {
                required: true,
                email: true
            }
        },
        messages: {
            PlainName: {
                required: i18n.get("your-full-name-required")
            },
            EMail: {
                required: i18n.get("your-mail-address-required"),
                email: i18n.get("your-mail-address-invalid")
            }
        },
        errorPlacement: function ($error, $element) {
            var name = $element.attr("name");
            $("#error" + name).text($error.text());
        }
    });
    //--
    $("#profile-common-form").find("#PlainName, #EMail, #Motto, #Description").change(function () {
        var $field = $(this);
        if ($field.valid()) {

            member.updateField(
                $field.attr("id"),
                $field.val(),
                $field.data("i18n-placeholder"),
                function () {
                    if ($field.attr("id") === "PlainName") {
                        $("header").find("h1.member").html($field.val().replace(" ", "<br>"));
                    }
                }
            );
        }
    });

    //AVATAR
    initDropZone("#profile-avatar-form",
        "/api/account/avatarimage", 200, 200,
        "upload-avatar-error",
        function (data) {
            var urlSuffix = ((data.Avatar100Url.indexOf("?") === -1) ? "?r=" : "&r=") + randomString(5);
            $("header .avatar").css("background-image", "url('" + data.Avatar100Url + urlSuffix + "')");
            $("#profile-avatar-form .current img").prop("src", data.Avatar200Url + urlSuffix);
            toastMessageText(i18n.get("field-update-message").replace("%FIELD%", i18n.get("your-avatar-image")));
        }
    );

    //HEADER-IMAGE
    initDropZone("#profile-header-image-form",
        "/api/account/header",
        $("#profile-header-image-form").width(), $("#profile-header-image-form").height(),
        "upload-header-image-error",
        function (data) {
            var urlSuffix = ((data.Header640Url.indexOf("?") === -1) ? "?r=" : "&r=") + randomString(5);
            member.Header640Url = data.Header640Url + urlSuffix;
            member.Header1024Url = data.Header1024Url + urlSuffix;
            member.Header1440Url = data.Header1440Url + urlSuffix;
            member.Header1920Url = data.Header1920Url + urlSuffix;
            member.HeaderColor = data.HeaderColor;
            $("#member-header-style").text($(member.getHeaderStyle()).text());
            $("#profile-header-image-form .current img").prop("src", data.Header640Url + urlSuffix);
            $("body").css("background-color", "rgb(" + member.HeaderColor + ")");
            toastMessageText(i18n.get("field-update-message").replace("%FIELD%", i18n.get("your-header-image")));
        }
    );

    //HEADER-COLOR
    $("#header-color-reset").click(function () {
        resetHeaderColor(member); return false;
    });
    //--
    $("#HeaderColor").change(function () {
        var colorHex = $(this).val(),
            colorR = hexToR(colorHex),
            colorG = hexToG(colorHex),
            colorB = hexToB(colorHex);

        member.updateField("HeaderColor", (colorR + "," + colorG + "," + colorB), "your-header-color", function () {
            $("#header-color-label").text(colorHex);
            $("body").css("background-color", colorHex);
        });
    });

    //SOCIAL-MEDIAS
    $("#profile-social-media-form").validate({
        errorPlacement: function ($error, $element) {
            var name = $element.attr("name");
            $("#error" + name).text($error.text());
        }
    });
    //--
    $('[name*="sm-"]').each(function () {
        $(this).rules('add', {
            url: true,
            messages: {
                url: i18n.get("social-media-url-invalid"),
            }
        });
    });
    //--
    $("#profile-social-media-form").find(".social-media").change(function () {
        var $field = $(this);
        if ($field.valid()) {
            var model = {
                "Id": $field.data("id"),
                "MemberId": member.Id,
                "Type": $field.data("type"),
                "Url": $field.val()
            };
            if (model.Id === 0) {
                executeSteamAction("new-social-media", null, model,
                    function (result) {
                        $field.data("id", result.Data.Id);
                        toastMessage("social-media-message");
                    },
                    function (result) { toastErrorText(result, "social-media-error"); }
                );
            } else {
                if (model.Url.length !== 0) {
                    executeSteamAction("save-social-media", null, model,
                        function () { toastMessage("social-media-message"); },
                        function (result) { toastErrorText(result, "social-media-error"); }
                    );
                } else {
                    executeSteamAction("delete-social-media", { Id: model.Id }, null,
                        function () {
                            $field.data("id", 0);
                            toastMessage("social-media-delete-message");
                        },
                        function (result) { toastErrorText(result, "social-media-delete-error"); }
                    );
                }
            }
        }
    });

    //TOPICS, LOCATIONS, EVENTS
    $(".collection-items").mCustomScrollbar({
        theme: "dark-thick",
        scrollInertia: 200
    });

    //TOPICS      
    $("#topic-list .collection-item").click(function () {
        editTopic($(this).data("id"), member); return false;
    });
    $("#topic-edit > p").find("button").click(function () {
        newTopic(member);
    });

    //LOCATIONS
    $("#location-list .collection-item").click(function () {
        editLocation($(this).data("id"), member); return false;
    });
    $("#location-edit > p").find("button").click(function () {
        newLocation(member);
    });

    //EVENTS
    $("#event-list .collection-item").click(function () {
        editEvent($(this).data("id"), member); return false;
    });
    $("#event-edit > p").find("button").click(function () {
        newEvent(member);
    });

    //NEW-PHOTOS
    var fotos = Enumerable
        .From(member.Fotos)
        .Where(function (x) { return x.IsNew; })
        .OrderBy(function (x) { return x.PublishDate; })
        .Select()
        .ToArray();
    //--
    $.each(fotos, function (index, foto) {
        showNewPhoto(foto);
    });
    
    //NEW-PHOTO-UPLOAD
    initDropZone("#foto-upload-form",
        "/api/data/photo", 300, 300,
        "newphotos-upload-error",
        function (data) {
            showNewPhoto(new Foto(data, member));
            toastMessage("newphotos-upload-message");
        }
    );

    $("body").css("background-color", "rgb(" + member.HeaderColor + ")");

    fittingContent();
    setTooltips($(".details-commands"), "left");


    
}
// ---------------------------------------------
function editProfileMap(jE, lat, lng, jAddress, member, fUpdate) {
    var _epm = this;

    if (global_online) {

        jE.gmap3({
            map: {
                options: gmapOptions(lat, lng, true, (lat === 0 && lng === 0) ? 2 : 10)
            },
            marker: {
                latLng: [lat, lng],
                options: {
                    draggable: true
                },
                events: {
                    dragend: function (marker) {
                        _epm.updateMarkerInfo(marker);
                    },
                    click: function (marker) {
                        _epm.updateMarkerInfo(marker);
                    }
                }
            }
        });
    }
    this.updateMarkerInfo = function (marker) {
        jE.gmap3({
            getaddress: {
                latLng: marker.getPosition(),
                callback: function (results) {
                    var map = $(this).gmap3("get");
                    map.setZoom(10);

                    //Info-Window
                    var infowindow = $(this).gmap3({ get: "infowindow" }),
                        content = (results && results[0]) ? results[0].formatted_address : "no address";

                    if (jAddress.length !== 0) {
                        jAddress.val(content);
                    }

                    if (infowindow) {
                        infowindow.open(map, marker);
                        infowindow.setContent(content);
                    } else {
                        $(this).gmap3({
                            infowindow: {
                                anchor: marker,
                                options: { content: content }
                            }
                        });
                    }
                    var location = new Location({ Id: 0, MemberId: member.Id }, member);
                    location.fillFromGeoResults(results, function() {
                        fUpdate(location);
                    });

                }
            }
        });
    };
    this.setAddress = function (sAddress) {
        jE.gmap3({
            getlatlng: {
                address: sAddress,
                callback: function (results) {
                    if (!results) return;
                    var latLng = results[0].geometry.location;
                    jE.gmap3({
                        get: {
                            name: 'marker',
                            callback: function (marker) {
                                marker.setPosition(latLng);
                                _epm.updateMarkerInfo(marker);
                            }
                        }
                    });
                }
            }
        });
    };
}
// ---------------------------------------------
function initDropZone(e, actionUrl, w, h, errorMsgLocale, fSuccess) {

    var jE = $(e);

    var dz = new Dropzone(e, {
        url: actionUrl,
        paramName: "file",
        acceptedFiles: ".jpg,.jpeg",
        maxFiles: 10,
        maxFilesize: 20, // MB (siehe auch web.config > maxRequestLength in KByte und maxAllowedContentLength in Byte)
        thumbnailWidth: w,
        thumbnailHeight: h,
        dictDefaultMessage: i18n.get("drop-file-here"),
        dictInvalidFileType: i18n.get("invalid-file-type"),
        addedfile: function (file) {
            this.options.url = actionUrl + "/" + UrlSafeFileName(file.name);
            removeCompleted(0); //alte wegräumen
            $("body").toggleClass("wait");
        },
        success: function (file, response) {
            if (response.Status && response.Status.Code !== 0) {
                var errorLocale = getErrorLocale(response.Status.Code);
                if (errorLocale) {
                    toastError(errorLocale, errorMsgLocale);
                } else {
                    toastError(response.Status.Message, errorMsgLocale);
                }
                removeCompleted(2000); //bei Server-Fehler wegräumen
            } else {
                fSuccess(response.Data);
                removeCompleted(2000); //fertige wegräumen
            }
            $("body").toggleClass("wait");
        },
        error: function (file, response) {
            toastErrorText(response, errorMsgLocale);
            if (file.status === "error") {
                removeCompleted(2000); //fehlerhafte wegräumen
                $("body").toggleClass("wait");
            }
        }
    });

    function removeCompleted(timeout) {
        setTimeout(function () {
            jE.find(".dz-preview.dz-complete").fadeOut("slow", function () {
                $(this).remove();
            });
        }, timeout);
    }

    return dz;
}
// ---------------------------------------------
function resetHeaderColor(member) {
    
    $("body").toggleClass("wait");
    executeSteamAction("header-color-reset", null, null,
        function (result) {
            var arrColor = result.Data.split(","),
                colorR = arrColor[0],
                colorG = arrColor[1],
                colorB = arrColor[1],
                colorHex = "#" + rgbToHex(colorR, colorG, colorB);

            $("#HeaderColor").val(colorHex);
            $("#header-color-label").text(colorHex);
            $("body").css("background-color", "rgb(" + result.Data + ")");
            toastMessageText(i18n.get("header-reset-color-message"));
            $("body").toggleClass("wait");
        },
        function (result) {
            toastErrorText(result, "header-reset-color-error");
            $("body").toggleClass("wait");
        }
    );
}
// ---------------------------------------------
function editTopic(id, member) {

    $("#topic-list .collection-items").find(".edit").removeClass("edit");

    var topic = Enumerable
        .From(member.Topics)
        .Where(function(x) { return x.TopicId === id; }).FirstOrDefault();

    var sTopic = i18n.translate(fsTemplates.tEditTopic(topic)),
        jTopic = $(sTopic);

    initTopicForm(jTopic);

    jTopic.find("#TopicName, #TopicDescription").change(function() {
        var $field = $(this);
        if ($field.valid()) {

            updateModel("Topic", id,
                $field.attr("id").replace("Topic", ""),
                $field.val(),
                $field.data("i18n-placeholder"),
                function() {
                    topic[$field.attr("id")] = $field.val();
                    topic.refresh(function() {
                        jTopic.find("#TopicUrl").val(topic.TopicUrl);
                    });
                    if ($field.attr("id") === "TopicName") {
                        $("#topic-list .collection-items").find("#topic-" + id).find("span").text($field.val());
                    }

                }
            );
        }
    });

    var jGrid;
    if (topic.TopicPhotoCount !== 0) {
        var fotos = Enumerable
            .From(member.Fotos)
            .Where(function(x) { return x.HasTopic(topic.TopicKey); })
            .Select()
            .ToArray();

        jGrid = $(fsTemplates.tThumbGrid({ fotos: fotos }));
        jGrid.mCustomScrollbar({ theme: "dark-thick", scrollInertia: 200 });
    }

    jTopic.find("button").click(function() {
        popoutConfirm((topic.TopicPhotoCount !== 0) ? "topic-delete-connected" : "topic-delete",
            function() {
                executeSteamAction("delete-topic", { Id: topic.TopicId }, null,
                    function() {
                        $("#topic-list .collection-items").find("#topic-" + topic.TopicId).fadeOut(function() { $(this).remove(); });
                        $("#topic-edit > div").fadeOut(function() { $(this).empty(); });
                    },
                    function(result) { toastErrorText(result, "topic-delete-error"); }
                );
            }
        );
        return false;
    });

    $("#topic-edit > div").fadeOut(function() {
        $(this).empty().append(jTopic).fadeIn();
        if (jGrid) {
            $(this).append($('<a href="' + topic.TopicUrl + '"><h2 style="clear: both;">' + topic.TopicPhotoCount + ' ' + i18n.get("photos") + '</h2></a>')).append(jGrid);
        }
    });

    $("#topic-list .collection-items").find("#topic-" + id).addClass("edit");

    disableEnter(jTopic);
}

//---
function newTopic(member) {

    $("#topic-list .collection-items").find(".edit").removeClass("edit");

    var sTopic = i18n.translate(fsTemplates.tNewTopic()),
        jTopic = $(sTopic);

    initTopicForm(jTopic);

    jTopic.find("button").click(function () {
        if ($("#topic-new-form").valid()) {

            var model = {
                "Name": $("#TopicName").val(),
                "Description": $("#TopicDescription").val(),
                "PhotoCount": 0,
                "MemberId": member.Id
            };

            executeSteamAction("new-topic", null, model,
                function (result) {
                    toastMessageText(i18n.get("topic-new-message"));
                    var newTopic = new Topic(result.Data, member);
                    member.Topics.push(newTopic);

                    var newItem = $('<a></a>');
                    newItem.prop("id", "topic-" + result.Data.Id);
                    newItem.prop("href", "#");
                    newItem.data("id", result.Data.Id);
                    newItem.addClass("collection-item").addClass("collection-topic");
                    newItem.append('<span>' + result.Data.Name + '</span>');
                    newItem.append('<i>0</i>');
                    newItem.click(function () {
                        editTopic(result.Data.Id, member); return false;
                    });

                    $("#topic-list .collection-items .mCSB_container").prepend(newItem);
                    editTopic(result.Data.Id, member);
                },
                function (result) {
                    toastErrorTextAndTitle(result, i18n.get("topic-new-error"));
                }
            );            
        }
        return false;
    });

    $("#topic-edit > div").fadeOut(function () {
        $(this).empty().append(jTopic).fadeIn();
    });
}
//---
function initTopicForm(jTopic) {
    jTopic.find(".input input[type='text']").Inputs();
    jTopic.find(".input textarea").Inputs();

    jTopic.validate({
        rules: {
            TopicName: { required: true }
        },
        messages: {
            TopicName: { required: i18n.get("topic-name-required") }
        },
        errorPlacement: function ($error, $element) {
            var name = $element.attr("name");
            $("#error" + name).text($error.text());
        }
    });
}
// ---------------------------------------------
function initLocationMap(id, member, jE, lat, lng, fStore) {
    var _ilm = this;
    //loadMap(jE, lat, lng, false);

    if (global_online) {

        jE.height(200).gmap3({
            map: {
                options: gmapOptions(lat, lng, true, (lat === 0 && lng === 0) ? 2 : 10)
            },
            marker: {
                latLng: [lat, lng],
                options: {
                    draggable: true
                },
                events: {
                    dragend: function (marker) {
                        _ilm.updateMarkerInfo(marker);
                    },
                    click: function (marker) {
                        _ilm.updateMarkerInfo(marker);
                    }
                }
            }
        });
        setTimeout(function () {
            jE.gmap3({ trigger: "resize" });
        }, 1000);
    }
    this.updateMarkerInfo = function (marker) {
        jE.gmap3({
            getaddress: {
                latLng: marker.getPosition(),
                callback: function (results) {
                    var map = $(this).gmap3("get");
                    map.setZoom(10);

                    var location = new Location({ Id: id, MemberId: member.Id }, member);
                    location.fillFromGeoResults(results, function () {
                        //val() und attr() wegen Inputs
                        $("#LocationName").val(location.LocationName).attr("value", location.LocationName);
                        $("#LocationLatitude").val(location.LocationLatitude).attr("value", location.LocationLatitude);
                        $("#LocationLongitude").val(location.LocationLongitude).attr("value", location.LocationLongitude);
                        $("#LocationStreet").val(location.LocationStreet).attr("value", location.LocationStreet);
                        $("#LocationCity").val(location.LocationCity).attr("value", location.LocationCity);
                        $("#LocationCounty").val(location.LocationCounty).attr("value", location.LocationCounty);
                        $("#LocationCountry").val(location.LocationCountry).attr("value", location.LocationCountry);
                        $("#LocationCountryIsoCode").val(location.LocationCountryIsoCode).attr("value", location.LocationCountryIsoCode);
                        if (id !== 0) {
                            fStore(location, function () {
                                $("#location-list .collection-items").find("#location-" + id).find("span").text(location.LocationName);
                            });
                        }                        
                    });
                }
            }
        });
    };
}
//---
function editLocation(id, member) {

    $("#location-list .collection-items").find(".edit").removeClass("edit");

    var location = Enumerable
        .From(member.Locations)
        .Where(function(x) { return x.LocationId === id; }).FirstOrDefault();

    var data = {
            Location: location,
            ChoosePlaceholder: i18n.get("choose-placeholder")
        },
        sLocation = i18n.translate(fsTemplates.tEditLocation(data)),
        jLocation = $(sLocation);

    initLocationForm(jLocation);

    //MAP
    initLocationMap(
        id, member,
        jLocation.find("#location-map"),
        location.LocationLatitude,
        location.LocationLongitude,
        function(loc, fSuccess) {
            executeSteamAction("save-location", null, loc.getPoco(),
                function(result) {
                    var resultLocation = new Location(result.Data, member);
                    member.replaceResultLocation(resultLocation);
                    toastMessage("location-save-message");
                    fSuccess();
                },
                function(result) { toastErrorText(result, "location-save-error"); }
            );
        }
    );

    //FIELD-CHANGE
    jLocation.find("#LocationName, #LocationLatitude, #LocationLongitude, #LocationStreet, #LocationCity, #LocationCounty, #LocationCountry, #LocationCountryIsoCode").change(function() {
        var $field = $(this);

        switch ($field.prop("id")) {
        case "LocationLatitude":
        case "LocationLongitude":
            if ($field.val().length === 0 || isNaN($field.val()) === true) {
                $field.val(0);
            }
            break;
        default:
        }

        if ($field.valid()) {

            updateModel("Location", id,
                $field.attr("id").replace("Location", ""),
                $field.val(),
                $field.data("i18n-placeholder"),
                function() {
                    location[$field.attr("id")] = $field.val();
                    location.refresh(function() {
                        jLocation.find("#LocationUrl").val(location.LocationUrl);
                    });
                    if ($field.attr("id") === "LocationName") {
                        $("#location-list .collection-items").find("#location-" + id).find("span").text($field.val());
                    }

                }
            );
        }
    });

    //FOTOS
    var jGrid;
    if (location.LocationPhotoCount !== 0) {
        var fotos = Enumerable
            .From(member.Fotos)
            .Where(function(x) { return (x.Location && x.Location.LocationId === location.LocationId); })
            .Select()
            .ToArray();

        jGrid = $(fsTemplates.tThumbGrid({ fotos: fotos }));
        jGrid.mCustomScrollbar({ theme: "dark-thick", scrollInertia: 200 });
    }

    //MERGE
    var jSelect = jLocation.find("#MergeLocationSelect");
    //--
    jLocation.find("#MergeLocation, #MergeLocationCancel").click(function() {
        var jMerge = jLocation.find("#merge-location-wrapper"),
            rebuild = !jMerge.is(":visible");

        if (rebuild) {
            var locToMerge = Enumerable.From(member.Locations)
                .Where(function(x) { return x.LocationId !== id; })
                .OrderBy(function(x) { return x.LocationName; }).ToArray();

            $.each(locToMerge, function(index, value) {
                jSelect.append('<option value="' + value.LocationId + '">' + value.LocationName + '</option>');
            });
            jSelect.val("").chosen({
                no_results_text: i18n.get("term-not-found"),
                width: "100%"
            });
        } else {
            jSelect.val("").trigger("chosen:updated").empty();
        }

        jLocation.find("#merge-location-wrapper").slideToggle();
        return false;

    });
    //--
    jLocation.find("#MergeLocationOk").click(function() {
        var model = {
                NewId: parseInt(jSelect.val()),
                OldId: parseInt(id)
            },
            photoCount = location.LocationPhotoCount;

        executeSteamAction("merge-location", null, model,
            function() {
                $("#location-list .collection-items").find("#location-" + id).fadeOut(function() { $(this).remove(); });
                $("#location-edit > div").fadeOut(function() { $(this).empty(); });
                $.each(member.Locations, function(index, value) {
                    if (value.LocationId === model.NewId) {
                        member.Locations[index].LocationPhotoCount += photoCount;
                        $("#location-list .collection-items").find("#location-" + model.NewId).find("i").html(member.Locations[index].LocationPhotoCount);
                        return false;
                    }
                });
                member.removeLocation(location.LocationId);
                $.scrollTo(0, 500);
            },
            function(result) { toastErrorText(result, "location-merge-error"); }
        );
        return false;
    });

    //DELETE
    jLocation.find("#DeleteLocation").click(function() {
        popoutConfirm((location.LocationPhotoCount !== 0) ? "location-delete-connected" : "location-delete",
            function() {
                executeSteamAction("delete-location", { Id: location.LocationId }, null,
                    function() {
                        $("#location-list .collection-items").find("#location-" + location.LocationId).fadeOut(function() { $(this).remove(); });
                        $("#location-edit > div").fadeOut(function() { $(this).empty(); });
                        member.removeLocation(location.LocationId);
                        $.scrollTo(0, 500);
                    },
                    function(result) { toastErrorText(result, "location-delete-error"); }
                );
            }
        );
        return false;
    });

    //--
    $("#location-edit > div").fadeOut(function() {
        $(this).empty().append(jLocation).fadeIn();
        if (jGrid) {
            $(this).append($('<a href="' + location.LocationUrl + '"><h2 style="clear: both;">' + location.LocationPhotoCount + ' ' + i18n.get("photos") + '</h2></a>')).append(jGrid);
        }
    });

    $("#location-list .collection-items").find("#location-" + id).addClass("edit");

    disableEnter(jLocation);
}
//---
function newLocation(member) {

    $("#location-list .collection-items").find(".edit").removeClass("edit");

    var sLocation = i18n.translate(fsTemplates.tNewLocation()),
        jLocation = $(sLocation);

    initLocationForm(jLocation);
    initLocationMap(0, member, jLocation.find("#location-map"), 0, 0, null);

    jLocation.find("button").click(function () {
        if ($("#location-new-form").valid()) {

            var model = {
                "Name": $("#LocationName").val(),
                "Latitude": $("#LocationLatitude").val() || 0,
                "Longitude": $("#LocationLongitude").val() || 0,
                "Street": $("#LocationStreet").val(),
                "City": $("#LocationCity").val(),
                "County": $("#LocationCounty").val(),
                "Country": $("#LocationCountry").val(),
                "CountryIsoCode": $("#LocationCountryIsoCode").val(),
                "PhotoCount": 0,
                "MemberId": member.Id
            };

            executeSteamAction("new-location", null, model,
                function (result) {
                    toastMessageText(i18n.get("location-new-message"));
                    var newLocation = new Location(result.Data, member);
                    member.Locations.push(newLocation);

                    var newItem = $('<a></a>');
                    newItem.prop("id", "location-" + result.Data.Id);
                    newItem.prop("href", "#");
                    newItem.data("id", result.Data.Id);
                    newItem.addClass("collection-item").addClass("collection-location");
                    newItem.append('<span>' + result.Data.Name + '</span>');
                    newItem.append('<i>0</i>');
                    newItem.click(function () {
                        editLocation(result.Data.Id, member); return false;
                    });

                    $("#location-list .collection-items .mCSB_container").prepend(newItem);
                    editLocation(result.Data.Id, member);
                    $.scrollTo(0, 500);
                },
                function (result) {
                    toastErrorTextAndTitle(result, i18n.get("location-new-error"));
                }
            );
        }
        return false;
    });

    $("#location-edit > div").fadeOut(function () {
        $(this).empty().append(jLocation).fadeIn();
    });
}
//---
function initLocationForm(jLocation) {
    jLocation.find(".input input[type='text']").Inputs();

    jLocation.validate({
        rules: {
            LocationName: { required: true },
            LocationCountry: { required: true },
            LocationCountryIsoCode: { required: true }
        },
        messages: {
            LocationName: { required: i18n.get("location-name-required") },
            LocationCountry: { required: i18n.get("location-country-required") },
            LocationCountryIsoCode: { required: i18n.get("location-country-iso-code-required") }
        },
        errorPlacement: function ($error, $element) {
            var name = $element.attr("name");
            $("#error" + name).text($error.text());
        }
    });
}
// ---------------------------------------------
function editEvent(id, member) {

    $("#event-list .collection-items").find(".edit").removeClass("edit");

    var event = Enumerable
        .From(member.Events)
        .Where(function (x) { return x.EventId === id; }).FirstOrDefault();

    var sEvent = i18n.translate(fsTemplates.tEditEvent(getDataEventForm(event, member))),
        jEvent = $(sEvent);

    initEventForm(jEvent);

    jEvent.find("#EventName, #EventDescription, #EventDate, #EventDateTo, #EventLocationId").change(function () {
        var $field = $(this);
        if ($field.valid()) {

            updateModel("Event", id,
                $field.attr("id").replace("Event", ""),
                $field.val(),
                $field.data("i18n-placeholder"),
                function () {
                    event[$field.attr("id")] = $field.val();
                    event.refresh(function () {
                        jEvent.find("#EventUrl").val(event.EventUrl);
                    });
                    if ($field.attr("id") === "EventName") {
                        $("#event-list .collection-items").find("#event-" + id).find("span").text($field.val());
                    }

                }
            );
        }
    });

    var jGrid;
    if (event.EventPhotoCount !== 0) {
        var fotos = Enumerable
            .From(member.Fotos)
            .Where(function (x) { return (x.Event && x.Event.EventId === event.EventId); })
            .Select()
            .ToArray();

        jGrid = $(fsTemplates.tThumbGrid({ fotos: fotos }));
        jGrid.mCustomScrollbar({ theme: "dark-thick", scrollInertia: 200 });
    }

    jEvent.find("button").click(function () {
        popoutConfirm((event.EventPhotoCount !== 0) ? "event-delete-connected" : "event-delete",
            function () {
                executeSteamAction("delete-event", { Id: event.EventId }, null,
                    function () {
                        $("#event-list .collection-items").find("#event-" + event.EventId).fadeOut(function () { $(this).remove(); });
                        $("#event-edit > div").fadeOut(function () { $(this).empty(); });
                    },
                    function (result) { toastErrorText(result, "event-delete-error"); }
                );
            }
        );
        return false;
    });

    $("#event-edit > div").fadeOut(function () {
        $(this).empty().append(jEvent).fadeIn();
        if (jGrid) {
            $(this).append($('<a href="' + event.EventUrl + '"><h2 style="clear: both;">' + event.EventPhotoCount + ' ' + i18n.get("photos") + '</h2></a>')).append(jGrid);
        }
    });

    $("#event-list .collection-items").find("#event-" + id).addClass("edit");

    disableEnter(jEvent);
}
//---
function newEvent(member) {

    $("#event-list .collection-items").find(".edit").removeClass("edit");

    var sEvent = i18n.translate(fsTemplates.tNewEvent(getDataEventForm(null, member))),
        jEvent = $(sEvent);

    initEventForm(jEvent);

    jEvent.find("button").click(function () {
        if ($("#event-new-form").valid()) {

            var model = {
                "Name": $("#EventName").val(),
                "Description": $("#EventDescription").val(),
                "Date": $("#EventDate").val(),
                "DateTo": $("#EventDateTo").val(),
                "LocationId": $("#EventLocationId").val(),
                "PhotoCount": 0,
                "MemberId": member.Id
            };

            executeSteamAction("new-event", null, model,
                function (result) {
                    toastMessageText(i18n.get("event-new-message"));

                    var newEvent = new Event(result.Data, member);
                    member.Events.push(newEvent);

                    var newItem = $('<a></a>');
                    newItem.prop("id", "event-" + result.Data.Id);
                    newItem.prop("href", "#");
                    newItem.data("id", result.Data.Id);
                    newItem.addClass("collection-item").addClass("collection-event");
                    newItem.append('<span>' + result.Data.Name + '</span>');
                    newItem.append('<i>0</i>');
                    newItem.click(function () {
                        editEvent(result.Data.Id, member); return false;
                    });

                    $("#event-list .collection-items .mCSB_container").prepend(newItem);
                    editEvent(result.Data.Id, member);
                },
                function (result) {
                    toastErrorTextAndTitle(result, i18n.get("event-new-error"));
                }
            );
        }
        return false;
    });

    $("#event-edit > div").fadeOut(function () {
        $(this).empty().append(jEvent).fadeIn();
    });
}
//---
function getDataEventForm(event, member) {

    var locations = null;
    
    locations = Enumerable
        .From(member.Locations)
        .OrderBy(function (x) { return x.LocationName; }).ToArray();
    $.each(locations, function (index, value) {
        value.Selected = (event !== null && event.EventLocation !== null && value.LocationId === event.EventLocation.Id);
    });

    return {
        Event: event,
        ChoosePlaceholder: i18n.get("choose-placeholder"),
        Locations: locations
    };
}
//---
function initEventForm(jEvent) {
    jEvent.find(".input input[type='text']").Inputs();
    jEvent.find(".input textarea").Inputs();

    jEvent.find("#EventLocationId").chosen({
        no_results_text: i18n.get("term-not-found"),
        width: "100%"
    });

    jEvent.find("#Date, #DateTo").change(function () {
        moment.locale(global_lang);
        var jOriginal = $(this),
            jHidden = $("#Event" + jOriginal.attr("name")),
            jError = $("#error" + jOriginal.attr("name")),
            vOriginal = jOriginal.val(),
            mToSave = moment(vOriginal, "DD.MM.YYYY");

        if (mToSave.isValid() || vOriginal.length === 0) {
            jOriginal.removeClass("error");
            jError.hide();
            //--
            jOriginal.val(mToSave.format("LL"));
            jHidden.val(mToSave.format("YYYY-MM-DD")).change();
        } else {
            jOriginal.addClass("error");
            jError.text(i18n.get("event-date-invalid")).show();
            jHidden.val(null);
        }
    });

    jEvent.validate({
        rules: {
            EventName: { required: true }
        },
        messages: {
            EventName: { required: i18n.get("event-name-required") }
        },
        errorPlacement: function ($error, $element) {
            var name = $element.attr("name");
            $("#error" + name).text($error.text());
        }
    });
}
// ---------------------------------------------
function syncPhotos() {
    
    var jCmd = $("#details-command-sync"),
        jCmdIcon = jCmd.find("i"),
        currClasses = jCmdIcon.prop("class");

    jCmd.addClass("running");
    jCmdIcon.prop("class", "icon icon-spin3 animate-spin");
    toastNotice("provider-sync-info");

    var http = new XMLHttpRequest(),
        lastIndex = 0,
        totalCount = 0;
    http.open('get', '/api/account/RefreshUserContent');
    http.onprogress = function () {
        var text = http.responseText.substring(lastIndex),
            result = jQuery.parseJSON(text);

        lastIndex = http.responseText.length;

        if (result.Data) {
            if (totalCount === 0 && result.Data.TotalFileCount !== 0) {
                totalCount = result.Data.TotalFileCount;
                $("#tablink-newphotos").append("<span>" + (result.Data.Index + 1) + " / " + totalCount + "</span>");
            } else {
                $("#tablink-newphotos").find("span").text((result.Data.Index + 1) + "/" + totalCount);
            }
            if (result.Data.Photo) {
                showNewPhoto(new Foto(result.Data.Photo, global_auth_member));
            }
        } else {
            toastErrorText(result.Status.Message, "provider-sync-error");
        }

    };
    http.onloadend = function () {
        if (totalCount !== 0) {
            $("#tablink-newphotos").addClass("attention").click(function () {
                $(this).removeClass("attention");
            }).find("span").remove();
        }
        jCmd.removeClass("running");
        jCmdIcon.prop("class", currClasses);
    };
    http.send(null);
}
// ---------------------------------------------
function showNewPhoto(foto) {
    
    //TODO: MultiEdit wie in OVERVIEW einbauen

    var data = {
        Foto: foto,
        SelectorTrigger: false
    },
    jFoto = $(i18n.translate(fsTemplates.tCubicle(data)));
    
    jFoto.find(".cubicle-link").unbind().click(function (e) {
        e.preventDefault();
        if ($("#content .collection-container").hasClass("selector")) {
            triggerSelector($(this), that);
        } else {
            window.location.href = $(this).attr("detailurl");
        }
        return false;
    });

    //jFoto.find(".selectortrigger a").click(function (e) {
    //    e.stopPropagation();
    //    triggerSelector($(this), that);
    //    return false;
    //});

    jFoto.find(".details").css("background-color", "rgba(" + foto.Color + ", 0.4)");
    jFoto.find(".title").css("background-color", "rgba(" + foto.Color + ", 1)");

    $("#new-photo-wrapper").prepend(jFoto);

    $("#newphotos-info-none").slideUp();
    $("#newphotos-info").slideDown();

}
//----------------------------------------------
function disableEnter(elementLocator) {
    var inputs = elementLocator.find("input[type='text']:enabled, textarea:enabled").keypress(function (e) {
        var key;

        if (window.event)
            key = window.event.keyCode; //IE
        else
            key = e.which;

        if (key === 13) {
            e.preventDefault();
            var nextInput = inputs.get(inputs.index(this) + 1);
            if (nextInput) {
                nextInput.focus();
            } else {
                inputs.get(0).focus();
            }
        }
    });
}