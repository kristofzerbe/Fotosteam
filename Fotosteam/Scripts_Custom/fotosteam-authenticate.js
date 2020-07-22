loginProvider = {
    Google: 0,
    Twitter: 1,
    Facebook: 2,
    Microsoft: 3
};
storageProvider = {
    Dropbox: 0,
    GoogleDrive: 1,
    OneDrive: 2
};

var _token,
    _windowToClose,
    _isRegistering,
    _currentLoginProvider,
    _isAuthorizing,
    _currentStorageProviderKey;

var windowSettings = function() {
    var w = 1000,
        h = 600,
        l = (screen.width / 2) - (w / 2),
        t = (screen.height / 2) - (h / 2);
    return "toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=" + w + ", height=" + h + ", top=" + t + ", left=" + l;
};

// =============================================
function authenticate(loginProviderKey) {

    var url = window.location.origin;

    switch (loginProviderKey) {
        case loginProvider.Google:      _currentLoginProvider = 'Google';    break;
        case loginProvider.Twitter:     _currentLoginProvider = 'Twitter';   break;
        case loginProvider.Facebook:    _currentLoginProvider = 'Facebook';  break;
        case loginProvider.Microsoft:   _currentLoginProvider = 'Microsoft'; break;
        default:                        _currentLoginProvider = null;
    }

    if (!_currentLoginProvider) {
        toastError("authenticate-unknown-provider", "authentication-error"); return;
    }

    _token = createGuid();

    var externalProviderUrl = url +
        "/api/authorize/login?provider=" + _currentLoginProvider +
        "&token=" + _token +
        "&redirect_uri=" + url + "/AuthenticationReady.html";

    _windowToClose = window.open(
        externalProviderUrl, "Authenticate Account", windowSettings());
}
// ---------------------------------------------
function authorize(storageProviderKey) {

    var url = window.location.origin,
        apiUrl = "",
        redirectUrl = "";

    switch (storageProviderKey) {
        case storageProvider.Dropbox:
            apiUrl = "/api/authorize/DropboxAuthorisationUrl";
            redirectUrl = url;
            break;

        case storageProvider.GoogleDrive:
            apiUrl = "/api/authorize/GetDriveAuthorizationUrl";
            redirectUrl = url + "/api/authorize/authorizedrive/";
            break;

        case storageProvider.OneDrive:
            apiUrl = "/api/authorize/GetOneDriveAuthorizationUrl";
            redirectUrl = url + "/api/authorize/authorizeonedrive/";
            break;

        default:
    }

    _isAuthorizing = true;
    _currentStorageProviderKey = storageProviderKey;

    $.ajax({
        type: "POST",
        url: apiUrl,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(redirectUrl),
        dataType: "json",
        success: function (result) {
            if ($.isArray(result.Data)) {

                _token = result.Data[1];

                _windowToClose = window.open(
                    result.Data[0],
                    "AuthorizeController Provider", windowSettings());

            } else {
                if (result.Data === null) { toastError("authorization-already-authorized", "authorization-error"); return; }

                _windowToClose = window.open(
                    result.Data,
                    "AuthorizeController Provider", windowSettings());
            }
        },
        error: function (result) {
            var response = result.responseText;
            toastError(response, "authorization-error");
        }
    });

}
// ---------------------------------------------
function closeAuthWindow() {

    _windowToClose.close();
    _windowToClose = null;

    var url;

    if (_isAuthorizing) {
        if (_currentStorageProviderKey === storageProvider.Dropbox) {
            url = "/api/authorize/DropBoxVerify/"; //Special-Roundtrip für Dropbox mit Rückgabe des vollständigen Members
        } else {
            url = "/api/account/MemberInfo/"; //auf normalem Weg den nun vollständigen Member holen
        }
    } else {
        url = "/api/authorize/VerifyLogin/";
    }

    checkForAuthResult(_token, url);
}

function checkForAuthResult(id, url) {

    if (id) { url += id;}

    $.ajax({
        type: "GET",
        url: url,
        contentType: "application/json; charset=utf-8",
        data: "{}",
        dataType: "json",
        success: function (result) {

            if (_windowToClose !== null)
                _windowToClose.close();

            if (_isRegistering) {
                completeRegistration(result.Data);
            } else {
                displayAuthResult(result.Data);
            }
        },
        error: function (result) { 
            var response = result.responseText;
            if (_isAuthorizing) {
                toastError(response, "authorization-error");
            } else {
                toastError(response, "authentication-error");
            }
        }
    });
}
// ---------------------------------------------
function displayAuthResult(data) {

    if (data.Alias === "NOT SET") {
        window.location.reload();
    } else {
        window.global_auth_member = new Member(data);
        window.location.href = window.location.href;
    }
}
// =============================================
function register(loginProviderKey) {

    $("#Alias").removeData("previousValue");
    $("#InviteCode").removeData("previousValue");

    $.validator.addMethod("alphanumeric", function (value, element) {
        return this.optional(element) || /^[a-zA-Z0-9]+$/i.test(value);
    }, "Please enter letters only");

    $("#register-form").validate({
        rules: {
            PlainName: {
                required: true
            },
            Alias: {
                required: true,
                minlength: 4,
                alphanumeric: true,
                remote: {
                    url: "/api/account/IsAliasAvailable",
                    type: "post",
                    data: {
                        '': function () { //http://encosia.com/using-jquery-to-post-frombody-parameters-to-web-api/
                            return $("#Alias").val();
                        }
                    }
                }
            },
            EMail: {
                required: true,
                email: true
            },
            InviteCode: {
                required: true,
                remote: {
                    url: "/api/account/IsInviteCodeAvailable",
                    type: "post",
                    data: {
                        '': function () { //http://encosia.com/using-jquery-to-post-frombody-parameters-to-web-api/
                            return $("#InviteCode").val();
                        }
                    }
                }
            }
        },
        messages: {
            PlainName: {
                required: i18n.get("your-full-name-required")
            },
            Alias: {
                required: i18n.get("your-alias-name-required"),
                minlength: i18n.get("your-alias-name-invalid-length"),
                alphanumeric: i18n.get("your-alias-name-invalid-characters"),
                remote: i18n.get("your-alias-name-used")
            },
            EMail: {
                required: i18n.get("your-mail-address-required"),
                email: i18n.get("your-mail-address-invalid")
            },
            InviteCode: {
                required: i18n.get("your-invite-code-required"),
                remote: i18n.get("your-invite-code-invalid")
            }
        },
        errorPlacement: function ($error, $element) {
            var name = $element.attr("name");
            $("#error" + name).text($error.text());
        }
    });

    if ($("#register-form").valid()) {
        _isRegistering = true;
        authenticate(loginProviderKey);
    }

}
// ---------------------------------------------
function completeRegistration(member) {

    _isRegistering = false;

    var model = {
        "UserName": $("#PlainName").val(),
        "Alias": $("#Alias").val(),
        "Email": $("#EMail").val(),
        "InvitationCode": $("#InviteCode").val(),
        "Provider": _currentLoginProvider,
        "ExternalAccessToken": member.ProviderKey
        //TODO: "Language" :$("#....").val()
    };

    executeSteamAction("register-external", null, model,
        function(result) {
            displayRegisterResult(result.Data);
        },
        function(result) {
            toastErrorText(result, "authentication-error");
        }
    );
}
// ---------------------------------------------
function displayRegisterResult(data) {
    window.global_auth_member = new Member(data);
    window.location.href = window.location.origin + "/setup";
}
// =============================================
function signOff() {

    $.ajax({
        type: "GET",
        url: "/api/authorize/SignOut",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.Data === true) {
                window.global_auth_member = null;
                $.connection.hub.stop();
                window.location.reload();
            }
        }
    });
}
