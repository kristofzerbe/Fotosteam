
// =============================================
function Route(path) {
    var that = this;

    this.Path = path;
    this.FullPath = window.location.href;
    this.Title = "";
    this.PageKey = "";
    this.RouteToken = path.toLowerCase().split("/");

    var hashToken;
    var paramToken = this.RouteToken[this.RouteToken.length - 1].split("?");
    if (paramToken.length > 1) { //letztes Token wieder ohne Parameter einsetzen
        this.RouteToken[this.RouteToken.length - 1] = paramToken[0];
        hashToken = paramToken[1].split("#");
        this.Parameter = hashToken[0].split("&");
    } else {        
        hashToken = this.RouteToken[this.RouteToken.length - 1].split("#");
        this.RouteToken[this.RouteToken.length - 1] = hashToken[0];
    }
    this.HashParameter = hashToken[1];
    this.MainPath = this.RouteToken[1];

    // ---------------------------------------------
    this.Init = function() {

        switch (this.MainPath) {
            case "":
            case "index.html":
            case "start":
                new Start();
                break;

            case "legalinfo":
                new LegalInfo();
                break;

            case "setup":
            case "dashboard":
                if (global_auth_member) {
                    //window.global_member = global_auth_member;
                    if (global_auth_member.IsRegistered === true) {
                        window.global_auth_member.LoadDetailsAndRoute(function() {
                            that.goAuthMemberRoute();
                        });
                    } else {
                        new Start(function () {
                            //window.history.pushState({}, "", "/");
                            openSidebarBay('register', function () {
                                $("#sidebar-bay-register").find("#PlainName").focus().val(global_auth_member.PlainName).attr("value", global_auth_member.PlainName);
                            });
                        });
                    }
                } else {
                    new Start(function() {
                        window.history.pushState({}, "", "/");
                        openSidebarBay('login');
                    });
                }
                break;
            case "cc0":
                new CC0();
                break;

            default:
                //TODO: JS-Routing statt Url-Routing...
                //if (window.global_member && window.global_member.Steam === that.RouteToken[1]) {
                //    that.goMemberRoute(that.RouteToken[2]);
                //} else {
                    getSteamData("member", that.MainPath, function (resM) {
                        if (resM.Data !== null) {
                            window.global_member = new Member(resM.Data);
                            if (window.global_member.Steam) {
                                window.global_member.LoadDetailsAndRoute(function () {
                                    that.goMemberRoute(that.RouteToken[2]);
                                });
                            } else {
                                new FSError(404, that);
                            }
                        } else {
                            new FSError(404, that);
                        }
                    });
                //}
        }
    };
    // ---------------------------------------------
    this.goAuthMemberRoute = function () {

        switch (this.MainPath) {
            case "dashboard":

                if (global_auth_member.StorageAccessType !== 0) {
                    global_auth_member.ShowPage_Dashboard(that.HashParameter);
                    break;
                } else {
                    window.history.pushState({}, "", "/setup"); //ohne break, weiter mit setup
                }
                /* falls through */
            case "setup":

                if (global_auth_member.StorageAccessType !== 0) {
                    global_auth_member.ShowPage_Dashboard();
                    window.history.pushState({}, "", "/dashboard");
                } else {
                    global_auth_member.ShowPage_Setup();
                }
                break;
            default:
        }        
    };
    // ---------------------------------------------
    this.goMemberRoute = function(subPath) {
        
        switch (subPath) {
            case undefined:
            case "":
            case "about":
                global_member.ShowPage_About();
                break;

            case "journal":
                new Stack(StackTypes.JOURNAL);
                break;

            case "overview":
                new Overview(OverviewTypes.GRID);
                break;

            case "list":
                new Overview(OverviewTypes.LIST);
                break;

            case "foto":
                var fotoName = this.RouteToken[3];
                if (fotoName) {
                    var foto = Enumerable
                        .From(window.global_member.Fotos)
                        .Where(function(x) { return x.Name.toLowerCase() == fotoName.toLowerCase(); })
                        .Select()
                        .FirstOrDefault();

                    if (foto) {
                        foto.ShowPage();
                    } else {
                        new FSError(404, that);
                    }

                } else {
                    new FSError(404, that);
                }
                break;

            case "story":
                var storyKey = this.RouteToken[3];
                if (storyKey) {

                    var queryS = Enumerable
                        .From(window.global_member.Stories)
                        .Where(function(x) { return x.StoryKey == storyKey; })
                        .Select()
                        .FirstOrDefault();

                    if (queryS) {
                        queryS.ShowPage((this.RouteToken[4]) ? this.RouteToken[4] : null);
                    } else {
                        new FSError(404, that);
                    }
                }
                break;

            case "category":
                var category = (this.RouteToken[3]) ? this.RouteToken[3].toLowerCase() : "",
                    queryC = null;

                if (category) {
                    queryC = Enumerable
                        .From(window.global_member.Categories)
                        .Where(function(x) { return x.CategoryType == category; })
                        .Select()
                        .FirstOrDefault();
                }
                if (queryC) {
                    new Stack(StackTypes.CATEGORY, queryC);
                } else {
                    new FSError(404, that);
                }
                break;

            case "topic":
                var topic = (this.RouteToken[3]) ? this.RouteToken[3].toLowerCase() : "",
                    queryT = null;

                if (topic) {
                    queryT = Enumerable
                        .From(window.global_member.Topics)
                        .Where(function(x) { return x.TopicKey == topic; })
                        .Select()
                        .FirstOrDefault();
                }
                if (queryT) {
                    new Stack(StackTypes.TOPICS, queryT);
                } else {
                    new FSError(404, that);
                }
                break;

            case "location":
                var location = (this.RouteToken[3]) ? this.RouteToken[3].toLowerCase() : "",
                    queryL = null;

                if (location) {
                    queryL = Enumerable
                        .From(window.global_member.LocationsUsed)
                        .Where(function(x) { return x.LocationKey == location; })
                        .Select()
                        .FirstOrDefault();
                }
                if (queryL) {
                    new Stack(StackTypes.LOCATIONS, queryL);
                } else {
                    new FSError(404, that);
                }
                break;

            case "event":
                var event = (this.RouteToken[3]) ? this.RouteToken[3].toLowerCase() : "",
                    queryE = null;

                if (event) {
                    queryE = Enumerable
                        .From(window.global_member.EventsUsed)
                        .Where(function(x) { return x.EventKey == event; })
                        .Select()
                        .FirstOrDefault();
                }
                if (queryE) {
                    new Stack(StackTypes.EVENTS, queryE);
                } else {
                    new FSError(404, that);
                }
                break;

            default:
                new FSError(404, that);
            }
    };
    // ---------------------------------------------
    that.Init();
}