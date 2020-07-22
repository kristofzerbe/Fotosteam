// =============================================
function CommonBackground(json) {
    for (var property in json) { this[property] = json[property]; }
}
// =============================================
function Category(json, member) {
    for (var property in json) { this["Category" + property] = json[property]; }
    this.CategoryType = this.CategoryType.toLowerCase();
    this.CategoryName = i18n.get("category-" + this.CategoryType);
    this.CategoryUrl = member.SteamPath + "/category/" + this.CategoryType;
}
//---
function Categories(json, member) {
    var list = [];
    if (json) {
        list = json.map(function (x) {
            return new Category(x, member);
        });
    }
    return Enumerable.From(list).OrderBy(function (x) { return x.CategoryName; }).ToArray();
}
// =============================================
function Topic(json, member) {
    if (json) { for (var property in json) { this["Topic" + property] = json[property]; } }

    this.refresh = function(fDone) {
        this.TopicKey = MakeUrlSafe(this.TopicName);
        this.TopicUrl = member.SteamPath + "/topic/" + this.TopicKey;
        if (fDone !== undefined) { fDone(); }
    };
    this.refresh();
}
//---
function Topics(json, member) {
    var list = [];
    if (json) {
        list = json.map(function (x) {
            return new Topic(x, member);
        });
    }
    return list;
}

function LocationGroup(json, member) {
    if (json) {
        for (var property in json) {
            this["Location" + property] = json[property];
        }
    }
    if (this.LocationName) {
        this.LocationCaption = (this.LocationName.toUpperCase() === "NOTSET") ? i18n.get("notset") : this.LocationName;
        this.LocationKey = MakeUrlSafe(this.LocationName);
        this.LocationUrl = member.SteamPath + "/location/" + this.LocationKey;
    }
}

function LocationGroups(json, member) {
    var list = [];
    if (json) {
        list = json.map(function(x) {
            return new LocationGroup(x, member);
        });
    }
    return list;
}



// =============================================
function Location(json, member) {
    if (json) {
        for (var property in json) {
            this["Location" + property] = json[property];
        }
    }

    this.refresh = function (fDone) {
        if (this.LocationName) {
            this.LocationCaption = (this.LocationName.toUpperCase() === "NOTSET") ? i18n.get("notset") : this.LocationName;
            this.LocationKey = MakeUrlSafe(this.LocationName);
            this.LocationUrl = member.SteamPath + "/location/" + this.LocationKey;
        }
        //if (this.LocationCity) {
        //    this.LocationCaption = this.LocationCity + ", " + this.LocationCountry;
        //} else {
        //    this.LocationCaption = this.LocationCounty + ", " + this.LocationCountry;
        //}
        if (fDone !== undefined) { fDone(); }
    };    
    this.getPoco = function () {
        var skipProperties = ["LocationKey", "LocationUrl"], poco = {};
        for (var prop in this) {
            if (prop.startsWith("Location") && $.inArray(prop, skipProperties) === -1) {
                var s = prop.replace("Location", "");
                poco[s] = this[prop];
            }
        }
        return poco;
    };
    this.fillFromGeoResults = function(results, fDone) {
        this.LocationCountry = getGeoComponent(results, "country", "long_name");
        this.LocationCountryIsoCode = getGeoComponent(results, "country", "short_name");
        this.LocationState = getGeoComponent(results, "administrative_area_level_1", "long_name");
        this.LocationCounty = getGeoComponent(results, "administrative_area_level_2", "long_name");
        this.LocationCity = getGeoComponent(results, "locality", "long_name");
        this.LocationStreet = getGeoComponent(results, "route", "long_name");
        if (results[0].geometry) {
            this.LocationLatitude = results[0].geometry.location.lat();
            this.LocationLongitude = results[0].geometry.location.lng();
        }
        this.LocationName = (this.LocationCity.length !== 0 ? this.LocationCity : this.LocationCounty) + ", " + this.LocationCountry;
        fDone();
    };
    //--
    this.refresh();
}
//---
function Locations(json, member) {
    var list = [];
    if (json) {
        list = json.map(function (x) {
            return new Location(x, member);
        });
    }
    return list;
}
// =============================================
function Event(json, member) {
    if (json) {
        for (var property in json) {
            switch (property) {
                case "Date":
                    if (json[property]) {
                        this.EventDate = moment(json[property]).locale(global_lang).format("LL");
                    } else {
                        this.EventDate = "";
                    }
                    break;
                case "DateTo":
                    if (json[property]) {
                        this.EventDateTo = moment(json[property]).locale(global_lang).format("LL");
                    } else {
                        this.EventDateTo = "";
                    }
                    break;
                default:
                    this["Event" + property] = json[property];
            }
        }
    }

    this.refresh = function (fDone) {
        this.EventCaption = (this.EventName.toUpperCase() === "NOTSET") ? i18n.get("notset") : this.EventName;
        this.EventKey = MakeUrlSafe(this.EventName);
        this.EventUrl = member.SteamPath + "/event/" + this.EventKey;
        if (fDone !== undefined) { fDone(); }
    };
    this.refresh();
}
//---
function Events(json, member) {
    var list = [];
    if (json) {
        list = json.map(function (x) {
            return new Event(x, member);
        });
    }
    return list;
}
// =============================================
function Stories(json, member) {
    var list = [];
    if (json) {
        list = json.map(function (x) {
            return new Story(x, member);
        });
    }
    return list;
}
// =============================================
function Comment(json, ownedByAuthMember) {

    for (var property in json) {
        switch (property) {
            case "Date":
                this.Date = moment(json[property]).locale(global_lang).format("LLLL");
                break;
            case "UserAlias":
                this.UserUrl = (json[property].length !== 0) ? window.location.origin + "/" + json[property] : "";
                break;
            case "UserAvatarLink":
                this.UserAvatarLink = json[property];
                if (!this.UserAvatarLink) {
                    this.UserAvatarLink = "/Content/Icons/unknown_" + pad(randomNumber(1, 32), 2) + ".png";
                }
                break;
            case "Text":
                var mdText = Encoder.htmlDecode(json[property]);
                this.Text = markdown.Transform(mdText);
                break;
            default:
                this[property] = json[property];
        }
    }
    this.OwnedByAuthMember = ownedByAuthMember;
}
//---
function Comments(json, ownedByAuthMember) {
    var list = [];
    if (json) {
        list = json.map(function (x) {
            return new Comment(x, ownedByAuthMember);
        });
    }
    return list;
}
// =============================================
function Rating(json) {

    for (var property in json) {
        switch (property) {
            case "Date":
                this.Date = moment(json[property]).locale(global_lang).format("LLLL");
                break;
            case "UserAlias":
                this.UserUrl = (json[property].length !== 0) ? window.location.origin + "/" + json[property] : "";
                break;
            case "UserAvatarLink":
                this.UserAvatarLink = json[property];
                if (!this.UserAvatarLink) {
                    this.UserAvatarLink = "/Content/Icons/unknown_" + pad(randomNumber(1, 32), 2) + ".png";
                }
                break;
            default:
                this[property] = json[property];
        }
    }
}//---
function Ratings(json) {
    var list = [];
    if (json) {
        list = json.map(function (x) {
            return new Rating(x);
        });
    }
    return list;
}
// =============================================
function SocialMedia(json) {

    for (var property in json) { this[property] = json[property]; }

    switch (this.Type) {
        case 0:
            this.TypeClass = "website";
            this.TypeCaption = "Website";
            break;
        case 1:
            this.TypeClass = "facebook";
            this.TypeCaption = "Facebook";
            break;
        case 2:
            this.TypeClass = "gplus";
            this.TypeCaption = "Google+";
            break;
        case 3:
            this.TypeClass = "twitter";
            this.TypeCaption = "Twitter";
            break;
        case 4:
            this.TypeClass = "fivehundredpx";
            this.TypeCaption = "500px";
            break;
        case 5:
            this.TypeClass = "flickr";
            this.TypeCaption = "Flickr";
            break;
        case 6:
            this.TypeClass = "instagram";
            this.TypeCaption = "Instagram";
            break;
        case 7:
            this.TypeClass = "tumblr";
            this.TypeCaption = "Tumblr";
            break;
        case 8:
            this.TypeClass = "pinterest";
            this.TypeCaption = "Pinterest";
            break;
        default:
            this.TypeClass = "";
            this.TypeCaption = "";
    }
}
//---
function SocialMediaFullList(socialMedias) {

    var lst = socialMedias.slice(0);

    for (var i = 0; i <= 8; i++) {
        var found = false;

        for (var j = 0; j < lst.length; j++) {
            if (lst[j].Type == i) {
                found = true;
                break;
            }
        }
        if (!found) {
            lst.push(new SocialMedia({
                "Id": 0,
                "MemberId": 0,
                "Type": i,
                "Url": ""
            }));
        }
    }
    return lst;
}
//---
function SocialMedias(json) {
    var list = [];
    if (json) {        
        list = json.map(function (x) {
            return new SocialMedia(x);
        });
    }
    return Enumerable.From(list).OrderBy(function(x) { return x.Type; }).ToArray();
}
// =============================================
function Buddy(buddy, member) {
    for (var property in buddy) { this[property] = buddy[property]; }
    this.Member = member;
    this.MutualClass = this.IsMutual ? "" : "mutual";
}
//---
function Buddies(buddies, member) {
    var list = [];
    if (member.BuddyList) {
        list = member.BuddyList.map(function (x) {

            //1. alle, die member angefragt hat, d.h. MemberId
            //2. alle, die member angefragt haben, d.h. BuddyMemberId

            var m = Enumerable.From(buddies)
                .Where(function(y) {
                    return y.Id === x.MemberId || y.Id === x.BuddyMemberId;
                })
                .FirstOrDefault();
            return new Buddy(x, new Member(m));
        });
    }
    return list;
}
// =============================================
function NotificationGroup(json) {
    for (var property in json) {
        switch (property) {
            case "Notifications":
                this.Notifications = new Notifications(json[property]);
                break;
            default:
                this[property] = json[property];
        }
    }
    var caption = i18n.get("notificationgroup-type-" + NotificationTypes[this.Type]);

    switch (this.Type) {
        case 0: //general
        case 6: //dropbox-synchronization
        case 7: //photo-synch
            break;
        case 1: //comment
        case 2: //rating
        case 5: //buddy-new-photo
            caption = caption.replace("%1", this.Notifications.length);
            caption = caption.replace("%2", this.Notifications[0].PhotoName);
            break;
        case 3: //buddy-request
        case 4: //buddy-confirmation
            caption = caption.replace("%1", this.Notifications.length);
            break;
        default:
    }
    this.TypeCaption = caption;
}
//---
function NotificationGroups(json) {
    var list = [];
    if (json) {
        list = json.map(function (x) {
            return new NotificationGroup(x);
        });
    }
    return list;
}
//----------------------------------------------
function Notification(json) {
    for (var property in json) {
        switch (property) {
            case "Date":
                this.Date = moment(json[property]).locale(global_lang).format("LLL");
                break;
            case "UserAlias":
                this.UserAlias = json[property];
                this.UserUrl = (json[property].length !== 0) ? window.location.origin + "/" + json[property] : "";
                break;
            case "UserAvatarLink":
                this.UserAvatarLink = json[property];
                if (!this.UserAvatarLink) {
                    this.UserAvatarLink = "/Content/Icons/unknown_" + pad(randomNumber(1, 32), 2) + ".png";
                }
                break;
            default:
                this[property] = json[property];
        }
    }
    this.Counts = true;

    var caption = i18n.get("notification-type-" + NotificationTypes[this.Type]),
        url = null,
        imgurl = null;

    switch (this.Type) {
        case 0: //general
            caption = this.Data;
            break;
        case 1: //comment
        case 2: //rating
            caption = caption.replace("%1", this.UserAlias);
            caption = caption.replace("%2", this.PhotoTitle);
            url = global_auth_member.SteamPath + "/foto/" + this.PhotoName;
            imgurl = this.PhotoUrl;
            break;
        case 5: //buddy-new-photo
            caption = caption.replace("%1", this.UserAlias);
            caption = caption.replace("%2", this.PhotoTitle);
            url = this.UserUrl + "/foto/" + this.PhotoName;
            imgurl = this.PhotoUrl;
            break;
        case 3: //buddy-request
        case 4: //buddy-confirmation
            caption = caption.replace("%1", this.UserAlias);
            url = this.UserUrl + "/about";
            imgurl = this.UserAvatarLink;
            break;
        case 6: //dropbox-synchronization                    
            if (this.Data) { // für Push
                if (this.Data.Index === 0) {
                    //Start
                    caption = i18n.get("notification-type-dropbox-synch-start");
                    this.Counts = false;
                } else {
                    if (this.Data.Index !== this.Data.TotalFileCount) {
                        //Foto
                        caption = i18n.get("notification-photo-synced");
                        caption = caption.replace("%1", this.Data.Photo.Name);
                        caption = caption.replace("%2", this.Data.Index);
                        caption = caption.replace("%3", this.Data.TotalFileCount);
                        this.Counts = false;
                    } else {
                        //End
                        caption = i18n.get("notification-type-dropbox-synch-end");
                    }
                }
            } else { // für die Liste
                caption = i18n.get("notification-type-dropbox-synch-end");
            }
            break;
        case 7: //photo-synch
            if (this.Data) { // für Push
                if (this.Data.Index === 0) {
                    //Start
                    caption = i18n.get("notification-type-photo-synch-start");
                    this.Counts = false;
                } else {
                    if (this.Data.Index !== this.Data.TotalFileCount) {
                        //Foto
                        caption = i18n.get("notification-photo-synced");
                        caption = caption.replace("%1", this.Data.Photo.Name);
                        caption = caption.replace("%2", this.Data.Index);
                        caption = caption.replace("%3", this.Data.TotalFileCount);
                        this.Counts = false;
                    } else {
                        //End
                        caption = i18n.get("notification-type-photo-synch-end");
                    }
                }
            } else { // für die Liste
                caption = i18n.get("notification-type-photo-synch-end");
                url = "/dashboard";
            }
            break;
        default:
    }
    this.TypeCaption = caption;
    this.TypeUrl = url;
    this.TypeImageUrl = imgurl;
}
//---
function Notifications(json) {
    var list = [];
    if (json) {
        list = json.map(function (x) {
            return new Notification(x);
        });
    }
    return list;
}