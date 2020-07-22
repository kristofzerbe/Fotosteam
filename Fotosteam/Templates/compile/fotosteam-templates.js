this["fsTemplates"] = this["fsTemplates"] || {};

Handlebars.registerPartial("Comment", Handlebars.template({"1":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "            <a href=\""
    + escapeExpression(((helper = (helper = helpers.UserUrl || (depth0 != null ? depth0.UserUrl : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"UserUrl","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.UserName || (depth0 != null ? depth0.UserName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"UserName","hash":{},"data":data}) : helper)))
    + "</a>\r\n";
},"3":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "            <span>"
    + escapeExpression(((helper = (helper = helpers.UserName || (depth0 != null ? depth0.UserName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"UserName","hash":{},"data":data}) : helper)))
    + "</span>        \r\n";
},"5":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "        <button class=\"comment-delete mini shine\" data-id=\""
    + escapeExpression(((helper = (helper = helpers.CommentId || (depth0 != null ? depth0.CommentId : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"CommentId","hash":{},"data":data}) : helper)))
    + "\" data-i18n=\"comment-delete\"></button>\r\n";
},"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression, buffer = "<div class=\"comment\" id=\"comment-"
    + escapeExpression(((helper = (helper = helpers.CommentId || (depth0 != null ? depth0.CommentId : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"CommentId","hash":{},"data":data}) : helper)))
    + "\">\r\n    <img src=\""
    + escapeExpression(((helper = (helper = helpers.UserAvatarLink || (depth0 != null ? depth0.UserAvatarLink : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"UserAvatarLink","hash":{},"data":data}) : helper)))
    + "\">\r\n    <div>\r\n";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.UserUrl : depth0), {"name":"if","hash":{},"fn":this.program(1, data),"inverse":this.program(3, data),"data":data});
  if (stack1 != null) { buffer += stack1; }
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.OwnedByAuthMember : depth0), {"name":"if","hash":{},"fn":this.program(5, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "        <small>"
    + escapeExpression(((helper = (helper = helpers.Date || (depth0 != null ? depth0.Date : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Date","hash":{},"data":data}) : helper)))
    + "</small>\r\n        <div class=\"comment-text\">";
  stack1 = ((helper = (helper = helpers.Text || (depth0 != null ? depth0.Text : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Text","hash":{},"data":data}) : helper));
  if (stack1 != null) { buffer += stack1; }
  return buffer + "</div>\r\n    </div>\r\n</div>";
},"useData":true}));

Handlebars.registerPartial("ContactBay", Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "<div id=\"sidebar-bay-contact\" class=\"sidebar-bay\">\r\n    <div class=\"bay-wrapper\">\r\n        <a class=\"bay-close icon icon-angle-left\" onclick=\"closeSidebarBays();\"\r\n           data-i18n=\"contact\">Kontakt</a>\r\n        \r\n        <p data-i18n=\"contact-info\"></p>\r\n        \r\n        <form id=\"contact-form\">\r\n            <div class=\"field input dark\">\r\n                <input id=\"Subject\" name=\"Subject\" type=\"text\"\r\n                       data-i18n-placeholder=\"contact-subject\" />\r\n                <label for=\"Subject\" data-i18n=\"contact-subject\">Betreff</label>\r\n                <span id=\"errorSubject\" class='help-block'></span>\r\n            </div>\r\n            <div class=\"field input dark\">\r\n                <input id=\"SenderName\" name=\"SenderName\" type=\"text\"\r\n                       data-i18n-placeholder=\"your-name\" value=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.AuthMember : depth0)) != null ? stack1.PlainName : stack1), depth0))
    + "\" />\r\n                <label for=\"SenderName\" data-i18n=\"your-name\">SenderName</label>\r\n                <span id=\"errorSenderName\" class='help-block'></span>\r\n            </div>\r\n            <div class=\"field input dark\">\r\n                <input id=\"SenderMail\" name=\"SenderMail\" type=\"email\"\r\n                       data-i18n-placeholder=\"your-mail-address\" value=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.AuthMember : depth0)) != null ? stack1.Email : stack1), depth0))
    + "\" />\r\n                <label for=\"SenderMail\" data-i18n=\"your-mail-address\">EMail</label>\r\n                <span id=\"errorSenderMail\" class='help-block'></span>\r\n            </div>\r\n            <div class=\"field input dark\">\r\n                <textarea id=\"Message\" name=\"Message\" data-i18n-placeholder=\"contact-message\"></textarea>\r\n                <label for=\"Message\" data-i18n=\"contact-message\">Nachricht</label>\r\n                <span id=\"errorMessage\" class='help-block'></span>\r\n            </div>\r\n\r\n            <button class=\"button expand icon icon-mail\" data-i18n=\"contact-send\" onclick=\"sendContact('mail'); return false;\"></button>\r\n            <button class=\"button expand icon icon-trello\" data-i18n=\"contact-trello\" onclick=\"sendContact('trello'); return false;\"></button>\r\n        </form>        \r\n        \r\n        <p data-i18n=\"contact-trello-info\"></p>\r\n\r\n    </div>\r\n</div>";
},"useData":true}));

Handlebars.registerPartial("FotoExif", Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression, buffer = "<div class=\"details-panel panel-exif\">\r\n    <h3 data-i18n=\"exif-data\">EXIF-Daten der Kamera</h3>\r\n    <div class=\"flex-table\">\r\n        \r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"capture-date\">Aufnahmedatum</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.CaptureDate : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"manufacturer\">Hersteller</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.Make : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"model\">Modell</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.Model : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"focal-length\">Brennweite</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.FocalLength : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"aperture\">Blende</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.Aperture : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"exposure-time\">Belichtungszeit</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.ExposureTime : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"exposure-bias\">Belichtungskorrektur</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.ExposureBiasValue : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"exposure-program\">Belichtungsprogramm</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.ExposureProgram : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"exposure-mode\">Belichtungsmodus</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.ExposureMode : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"iso-speed-rating\">ISO-Lichtempfindlichkeit</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.ISOSpeedRatings : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"metering-mode\">Messmethode</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.MeteringMode : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div id=\"ExifExtendedDetailsButtons\" onclick=\"showExtendedExif(); return false;\" class=\"flex-cell icon icon-dot-3\" style=\"cursor: pointer;\">\r\n        </div>        \r\n        <div id=\"ExifExtendedDetails\" style =\"display: none;\">\r\n            <div class=\"flex-cell\" style =\"display: none;\"></div>\r\n            <div class=\"flex-cell\">\r\n                <strong data-i18n=\"artist\">Autor</strong>\r\n                <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.Artist : stack1), depth0))
    + "</span>\r\n            </div>\r\n            <div class=\"flex-cell\">\r\n                <strong data-i18n=\"file-name\">Dateiname</strong>\r\n                <span>"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Name : stack1), depth0))
    + "</span>\r\n            </div>\r\n            <div class=\"flex-cell\">\r\n                <strong data-i18n=\"copyright\">Copyright</strong>\r\n                <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.Copyright : stack1), depth0))
    + "</span>\r\n            </div>\r\n            <div class=\"flex-cell\">\r\n                <strong data-i18n=\"description\">Beschreibung</strong>\r\n                <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.Description : stack1), depth0))
    + "</span>\r\n            </div>\r\n            <div class=\"flex-cell\">\r\n                <strong data-i18n=\"software\">Software</strong>\r\n                <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.Software : stack1), depth0))
    + "</span>\r\n            </div>\r\n            <div class=\"flex-cell\">\r\n                <strong data-i18n=\"resolution\">Bildauflösung</strong>\r\n                <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.Resolution : stack1), depth0))
    + "</span>\r\n            </div>\r\n            <div class=\"flex-cell\">\r\n                <strong data-i18n=\"resolution-unit\">Auflösungseinheit</strong>\r\n                <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.ResolutionUnit : stack1), depth0))
    + "</span>\r\n            </div>\r\n            <div class=\"flex-cell\">\r\n                <strong data-i18n=\"orientation\">Bildformat</strong>\r\n                <span>"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.OrientationLocale : stack1), depth0))
    + "</span>\r\n            </div>\r\n            <div class=\"flex-cell\">\r\n                <strong data-i18n=\"aspect-ratio\">Bildseitenverhältnis</strong>\r\n                <span>"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.AspectRatio : stack1), depth0))
    + "</span>\r\n            </div>\r\n            <div class=\"flex-cell\">\r\n                <strong data-i18n=\"longitude\">GPS-Längengrad</strong>\r\n                <span>";
  stack1 = lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.LongitudeDeg : stack1), depth0);
  if (stack1 != null) { buffer += stack1; }
  buffer += "</span>\r\n            </div>\r\n            <div class=\"flex-cell\">\r\n                <strong data-i18n=\"latitude\">GPS-Breitengrad</strong>\r\n                <span>";
  stack1 = lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.LatitudeDeg : stack1), depth0);
  if (stack1 != null) { buffer += stack1; }
  return buffer + "</span>\r\n            </div>\r\n        </div>\r\n    </div>\r\n    <div class=\"exif-map\"></div>\r\n</div>\r\n";
},"useData":true}));

Handlebars.registerPartial("FotoRating", Handlebars.template({"1":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "            <div class=\"rating-wrapper\">\r\n                <a class=\"icon icon-star rating-3\" href=\"#\" onclick=\"rateFoto(this, '"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.fId : stack1), depth0))
    + "', "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Id : stack1), depth0))
    + ", 3); return false;\"><span data-i18n=\"rating-3\">Fantastisch!</span></a>\r\n                <a class=\"icon icon-star rating-2\" href=\"#\" onclick=\"rateFoto(this, '"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.fId : stack1), depth0))
    + "', "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Id : stack1), depth0))
    + ", 2); return false;\"><span data-i18n=\"rating-2\">Klasse</span></a>\r\n                <a class=\"icon icon-star rating-1\" href=\"#\" onclick=\"rateFoto(this, '"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.fId : stack1), depth0))
    + "', "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Id : stack1), depth0))
    + ", 1); return false;\"><span data-i18n=\"rating-1\">Interessant</span></a>\r\n            </div>\r\n";
},"3":function(depth0,helpers,partials,data) {
  return "            <div class=\"rating-wrapper\">\r\n                <a class=\"icon icon-star rating-3\" href=\"#\" onclick=\"openSidebarBay('login'); return false;\"><span data-i18n=\"rating-3\">Fantastisch!</span></a>\r\n                <a class=\"icon icon-star rating-2\" href=\"#\" onclick=\"openSidebarBay('login'); return false;\"><span data-i18n=\"rating-2\">Klasse</span></a>\r\n                <a class=\"icon icon-star rating-1\" href=\"#\" onclick=\"openSidebarBay('login'); return false;\"><span data-i18n=\"rating-1\">Interessant</span></a>\r\n            </div>\r\n            <p data-i18n=\"rating-login-info\" style=\"margin-top:20px;\"></p>\r\n";
  },"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression, buffer = "<div id=\"rating-popout-"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.fId : stack1), depth0))
    + "\" class=\"popout rating-popout\">\r\n    <div class=\"popout-wrapper\">\r\n        <div class=\"popout-content\" style=\"width: 360px\">\r\n            <h3 data-i18n=\"rating-photo\">Bewerte dieses Bild:</h3>\r\n\r\n";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.AuthMember : depth0), {"name":"if","hash":{},"fn":this.program(1, data),"inverse":this.program(3, data),"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer + "        </div>\r\n    </div>\r\n</div>";
},"useData":true}));

Handlebars.registerPartial("FotoSharing", Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "<div id=\"sharing-popout-"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.fId : stack1), depth0))
    + "\" class=\"popout sharing-popout\">\r\n    <div class=\"popout-wrapper\">\r\n        <div class=\"popout-content\" style=\"width: 330px\">\r\n            <h3>\r\n                <span data-i18n=\"share-it-on\">Teile dieses Bild auf</span><br />\r\n                <span class=\"sharing-platform\">...</span>\r\n            </h3>\r\n            <a class=\"button brand gplus icon icon-gplus\" href=\"#\"\r\n               onclick=\"sharingClick('"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.fId : stack1), depth0))
    + "', 'gplus'); return false;\"\r\n               onmouseover=\"sharingHover(this, 'Google Plus');\"\r\n               onmouseout=\"sharingHover(this, '...');\"></a>\r\n            <a class=\"button brand facebook icon icon-facebook\" href=\"#\"\r\n               onclick=\"sharingClick('"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.fId : stack1), depth0))
    + "', 'facebook'); return false;\"\r\n               onmouseover=\"sharingHover(this, 'Facebook');\"\r\n               onmouseout=\"sharingHover(this, '...');\"></a>\r\n            <a class=\"button brand twitter icon icon-twitter\" href=\"#\"\r\n               onclick=\"sharingClick('"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.fId : stack1), depth0))
    + "', 'twitter'); return false;\"\r\n               onmouseover=\"sharingHover(this, 'Twitter');\"\r\n               onmouseout=\"sharingHover(this, '...');\"></a>\r\n            <a class=\"button brand pinterest icon icon-pinterest\" href=\"#\"\r\n               onclick=\"sharingClick('"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.fId : stack1), depth0))
    + "', 'pinterest'); return false;\"\r\n               onmouseover=\"sharingHover(this, 'Pinterest');\"\r\n               onmouseout=\"sharingHover(this, '...');\"></a>\r\n            <a class=\"button brand tumblr icon icon-tumblr\" href=\"#\"\r\n               onclick=\"sharingClick('"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.fId : stack1), depth0))
    + "', 'tumblr'); return false;\"\r\n               onmouseover=\"sharingHover(this, 'Tumblr');\"\r\n               onmouseout=\"sharingHover(this, '...');\"></a>\r\n        </div>\r\n    </div>\r\n</div>";
},"useData":true}));

Handlebars.registerPartial("KeyboardBay", Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  return "<div id=\"sidebar-bay-keyboard\" class=\"sidebar-bay\">\r\n    <div class=\"bay-wrapper\">\r\n        <a class=\"bay-close icon icon-angle-left\" onclick=\"closeSidebarBays();\"\r\n           data-i18n=\"keyboard-help\">Tastaturhilfe</a>\r\n        <p data-i18n=\"keyboard-operable\">Diese ist Seite ist tastaturbedienar. Hier die Liste aller Tastaturkürzel:</p>\r\n        <div class=\"keycollection\"></div>\r\n    </div>\r\n</div>\r\n";
  },"useData":true}));

Handlebars.registerPartial("LoginBay", Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  return "<div id=\"sidebar-bay-login\" class=\"sidebar-bay\">\r\n    <div class=\"bay-wrapper\">\r\n        <a class=\"bay-close icon icon-angle-left\" onclick=\"closeSidebarBays();\"\r\n           data-i18n=\"login\">Login</a>\r\n\r\n        <p data-i18n=\"login-info\"></p>\r\n        \r\n        <p data-i18n=\"popup-info\" style=\"font-weight: bold\">Bitte Popup-Fenster dieser Website zulassen!</p>\r\n        \r\n        <p data-i18n=\"login-with\"></p>\r\n        <button class=\"button expand brand gplus\" onclick=\"return authenticate(loginProvider.Google);\"><i class=\"icon icon-gplus\"></i>... Google+</button>\r\n        <button class=\"button expand brand twitter\" onclick=\"return authenticate(loginProvider.Twitter);\"><i class=\"icon icon-twitter\"></i>... Twitter</button>\r\n        <button class=\"button expand brand facebook\" onclick=\"return authenticate(loginProvider.Facebook);\"><i class=\"icon icon-facebook\"></i>... Facebook</button>\r\n        <button class=\"button expand brand windows\" onclick=\"return authenticate(loginProvider.Microsoft);\"><i class=\"icon icon-windows\"></i>... Microsoft</button>\r\n\r\n        <p data-i18n=\"login-info-register\"></p>\r\n\r\n        <button class=\"button expand\" onclick=\"showSidebarBay('register'); return false;\"\r\n                data-i18n=\"register\">\r\n            Registrieren\r\n        </button>\r\n    </div>\r\n</div>\r\n";
  },"useData":true}));

Handlebars.registerPartial("NotificationBay", Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  return "<div id=\"sidebar-bay-notifications\" class=\"sidebar-bay\">\r\n    <div class=\"bay-wrapper\">\r\n        <a class=\"bay-close icon icon-angle-left\" onclick=\"closeSidebarBays();\"\r\n           data-i18n=\"notifications\">Mitteilungen</a>\r\n        <a id=\"dismiss-all-notifications\" href=\"#\"\r\n           class=\"tooltip-bottom icon icon-communication-clear-all\"\r\n           data-i18n-title=\"notifications-dismiss-all\"></a>\r\n        <ul id=\"sidebar-notifications\"></ul>\r\n    </div>\r\n</div>\r\n";
  },"useData":true}));

Handlebars.registerPartial("RegisterBay", Handlebars.template({"1":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "        <p data-i18n=\"loggedin-info\"\r\n           data-i18n-args=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.AuthMember : depth0)) != null ? stack1.ProviderUserMail : stack1), depth0))
    + "|"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.AuthMember : depth0)) != null ? stack1.ProviderName : stack1), depth0))
    + "\">\r\n        </p>\r\n";
},"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression, buffer = "<div id=\"sidebar-bay-register\" class=\"sidebar-bay\">\r\n    <div class=\"bay-wrapper\">\r\n        <a class=\"bay-close icon icon-angle-left\" onclick=\"closeSidebarBays();\"\r\n           data-i18n=\"register\">Registrieren</a>\r\n\r\n        <p data-i18n=\"register-info\"></p>\r\n        \r\n";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.AuthMember : depth0), {"name":"if","hash":{},"fn":this.program(1, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer + "\r\n        <form id=\"register-form\" style=\"margin: 10px 0 25px 0;\">\r\n            <div class=\"field input dark\">\r\n                <input id=\"PlainName\" name=\"PlainName\" type=\"text\"\r\n                       data-i18n-placeholder=\"your-full-name\" value=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.AuthMember : depth0)) != null ? stack1.ProviderUserName : stack1), depth0))
    + "\" />\r\n                <label for=\"PlainName\" data-i18n=\"your-full-name\">PlainName</label>\r\n                <span id=\"errorPlainName\" class='help-block'></span>\r\n            </div>\r\n            <div class=\"field input dark\">\r\n                <input id=\"Alias\" name=\"Alias\" type=\"text\"\r\n                       data-i18n-placeholder=\"your-alias-name\" />\r\n                <label for=\"Alias\" data-i18n=\"your-alias-name\">Alias</label>\r\n                <span id=\"errorAlias\" class='help-block'></span>\r\n            </div>\r\n            <div class=\"field input dark\">\r\n                <input id=\"EMail\" name=\"EMail\" type=\"email\"\r\n                       data-i18n-placeholder=\"your-mail-address\" value=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.AuthMember : depth0)) != null ? stack1.ProviderUserMail : stack1), depth0))
    + "\" />\r\n                <label for=\"EMail\" data-i18n=\"your-mail-address\">EMail</label>\r\n                <span id=\"errorEMail\" class='help-block'></span>\r\n            </div>\r\n            <div class=\"field input dark\">\r\n                <input id=\"InviteCode\" name=\"InviteCode\" type=\"text\"\r\n                       data-i18n-placeholder=\"your-invite-code\" />\r\n                <label for=\"InviteCode\" data-i18n=\"your-invite-code\"></label>\r\n                <span id=\"errorInviteCode\" class='help-block'></span>\r\n            </div>\r\n        </form>\r\n\r\n        <p data-i18n=\"popup-info\" class=\"icon icon-attention\"></p>\r\n\r\n        <p data-i18n=\"register-with\"></p>\r\n        <button class=\"button expand brand gplus\" onclick=\"return register(loginProvider.Google);\"><i class=\"icon icon-gplus\"></i>... Google+</button>\r\n        <button class=\"button expand brand twitter\" onclick=\"return register(loginProvider.Twitter);\"><i class=\"icon icon-twitter\"></i>... Twitter</button>\r\n        <button class=\"button expand brand facebook\" onclick=\"return register(loginProvider.Facebook);\"><i class=\"icon icon-facebook\"></i>... Facebook</button>\r\n        <button class=\"button expand brand windows\" onclick=\"return register(loginProvider.Microsoft);\"><i class=\"icon icon-windows\"></i>... Microsoft</button>\r\n    </div>\r\n</div>\r\n\r\n";
},"useData":true}));

Handlebars.registerPartial("SetupProvider", Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  return "<a class=\"brand logo dropbox\" href=\"#\" onclick=\"global_auth_member.SelectProvider(this, storageProvider.Dropbox); return false;\">\r\n    <img src=\"/Content/Images/Dropbox/logo-square-180.png\" alt=\"Dropbox\" />\r\n</a>\r\n<a class=\"brand logo googledrive\" href=\"#\" onclick=\"global_auth_member.SelectProvider(this, storageProvider.GoogleDrive); return false;\">\r\n    <img src=\"/Content/Images/GoogleDrive/logo-square-180.png\" alt=\"Google Drive\" />\r\n</a>\r\n<a class=\"brand logo onedrive\" href=\"#\" onclick=\"global_auth_member.SelectProvider(this, storageProvider.OneDrive); return false;\">\r\n    <img src=\"/Content/Images/OneDrive/logo-square-180.png\" alt=\"OneDrive\" />\r\n</a>";
  },"useData":true}));

Handlebars.registerPartial("SidebarCommonCommands", Handlebars.template({"1":function(depth0,helpers,partials,data) {
  return "<li>\r\n    <a class=\"cmd-bay\" href=\"#\" data-bay=\"login\">\r\n        <i class=\"icon icon-key\"></i>\r\n        <span data-i18n=\"login\">Login</span>\r\n    </a>\r\n</li>\r\n<li>\r\n    <a class=\"cmd-bay\" href=\"#\" data-bay=\"register\">\r\n        <i class=\"icon icon-user-add\"></i>\r\n        <span data-i18n=\"register\">Registrieren</span>\r\n    </a>\r\n</li>\r\n";
  },"3":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers.unless.call(depth0, (depth0 != null ? depth0.Registered : depth0), {"name":"unless","hash":{},"fn":this.program(4, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"4":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "<li>\r\n    <a class=\"cmd-bay\" href=\"#\" data-bay=\"register\"\r\n       data-i18n-title=\"loggedin-info\"\r\n       data-i18n-title-args=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.AuthMember : depth0)) != null ? stack1.ProviderUserMail : stack1), depth0))
    + "|"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.AuthMember : depth0)) != null ? stack1.ProviderName : stack1), depth0))
    + "\">\r\n        <i class=\"icon icon-user-add\"></i>\r\n        <span data-i18n=\"register\">Registrieren</span>\r\n    </a>\r\n</li>\r\n";
},"6":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.ShowMy : depth0), {"name":"if","hash":{},"fn":this.program(7, data),"inverse":this.program(9, data),"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"7":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "<li class=\"gap\">\r\n    <a class=\"\" href=\"/"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.AuthMember : depth0)) != null ? stack1.Steam : stack1), depth0))
    + "/about\">\r\n        <i class=\"icon icon-user\"></i>\r\n        <span data-i18n=\"my-fotosteam\">Mein Fotosteam</span>\r\n    </a>\r\n</li>\r\n<li>\r\n    <a class=\"cmd-notifications cmd-bay\" href=\"#\" data-bay=\"notifications\">\r\n        <i class=\"icon icon-bell\"></i>\r\n        <span data-i18n=\"notifications\">Mitteilungen</span>\r\n    </a>\r\n</li>\r\n<li>\r\n    <a class=\"\" href=\"/dashboard\">\r\n        <i class=\"icon icon-sliders\"></i>\r\n        <span data-i18n=\"dashboard\">Dashboard</span>\r\n    </a>\r\n</li>\r\n";
},"9":function(depth0,helpers,partials,data) {
  return "<li class=\"gap\">\r\n    <a class=\"cmd-notifications cmd-bay\" href=\"#\" data-bay=\"notifications\">\r\n        <i class=\"icon icon-bell\"></i>\r\n        <span data-i18n=\"notifications\">Mitteilungen</span>\r\n    </a>\r\n</li>\r\n<li>\r\n    <a class=\"\" href=\"/dashboard\">\r\n        <i class=\"icon icon-sliders\"></i>\r\n        <span data-i18n=\"dashboard\">Dashboard</span>\r\n    </a>\r\n</li>\r\n";
  },"11":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "<li>\r\n    <a class=\"\" href=\"#\" onclick=\"signOff(); return false;\"\r\n       data-i18n-title=\"loggedin-info\"\r\n       data-i18n-title-args=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.AuthMember : depth0)) != null ? stack1.ProviderUserMail : stack1), depth0))
    + "|"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.AuthMember : depth0)) != null ? stack1.ProviderName : stack1), depth0))
    + "\">\r\n        <i class=\"icon icon-off\"></i>\r\n        <span data-i18n=\"logout\">Logout</span>\r\n    </a>\r\n</li>\r\n";
},"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers.unless.call(depth0, (depth0 != null ? depth0.AuthMember : depth0), {"name":"unless","hash":{},"fn":this.program(1, data),"inverse":this.program(3, data),"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = (depth0 != null ? depth0.AuthMember : depth0)) != null ? stack1.IsRegistered : stack1), {"name":"if","hash":{},"fn":this.program(6, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.AuthMember : depth0), {"name":"if","hash":{},"fn":this.program(11, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer + "<!--<li class=\"gap\">\r\n    <a href=\"#\" data-i18n-title=\"search\">\r\n        <i class=\"foto-btn icon icon-search\" style=\"font-size: 0.6em;\"></i>\r\n    </a>\r\n    <i class=\"icon\" style=\"font-size: 0.6em;\">&nbsp;</i>\r\n    <input type=\"text\" style=\"width: 150px; background-color: white;\" id=\"searchItem\"/>\r\n</li>\r\n\r\n<li>\r\n    <a class=\"cmd-cc0\" href=\"/cc0\">\r\n        <i class=\"icon icon-CC\"></i>\r\n        <span data-i18n=\"license-free-photos\">CCO</span>\r\n    </a>\r\n</li>\r\n-->\r\n\r\n<li class=\"gap\">\r\n    <a class=\"cmd-lang lang-de\" href=\"#\" onclick=\"changeLanguage('de'); return false;\">\r\n        <i class=\"flag-icon flag-icon-de\"></i>\r\n        Deutsch\r\n    </a>\r\n</li>\r\n<li>\r\n    <a class=\"cmd-lang lang-en\" href=\"#\" onclick=\"changeLanguage('en'); return false;\">\r\n        <i class=\"flag-icon flag-icon-en\"></i>\r\n        English\r\n    </a>\r\n</li>\r\n\r\n<li class=\"gap\">\r\n    <a class=\"cmd-bay\" href=\"#\" data-bay=\"contact\">\r\n        <i class=\"icon icon-inbox\"></i>\r\n        <span data-i18n=\"contact\">Kontakt</span>\r\n    </a>\r\n</li>\r\n<li>\r\n    <a class=\"\" href=\"/legalinfo\">\r\n        <i class=\"icon icon-info-circled\"></i>\r\n        <span data-i18n=\"legalinfo\">Impressum</span>\r\n    </a>\r\n</li>\r\n\r\n<li class=\"gap\">\r\n    <a class=\"blog\" href=\"http://www.rokr.io\" target=\"_blank\">\r\n        <i class=\"icon icon-blogger\"></i>\r\n        <span>Blog</span>\r\n    </a>\r\n</li>\r\n<li>\r\n    <a class=\"gplus\" href=\"https://plus.google.com/+FotosteamPage\" target=\"_blank\">\r\n        <i class=\"icon icon-gplus\"></i>\r\n        <span>Google Plus</span>\r\n    </a>\r\n</li>\r\n<li>\r\n    <a class=\"facebook\" href=\"https://www.facebook.com/pages/Fotosteam/1597537423790954\" target=\"_blank\">\r\n        <i class=\"icon icon-facebook\"></i>\r\n        <span>Facebook</span>\r\n    </a>\r\n</li>\r\n<li>\r\n    <a class=\"twitter\" href=\"https://twitter.com/fotosteam\" target=\"_blank\">\r\n        <i class=\"icon icon-twitter\"></i>\r\n        <span>Twitter</span>\r\n    </a>\r\n</li>\r\n\r\n<li class=\"gap\">\r\n    <a class=\"cmd-bay\" href=\"#\" data-bay=\"keyboard\">\r\n        <i class=\"icon icon-keyboard\"></i>\r\n        <span data-i18n=\"keyboard-help\">Tastaturhilfe</span>\r\n    </a>\r\n</li>\r\n";
},"useData":true}));

Handlebars.registerPartial("StartAuthBrand", Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  return "<a href=\"http://plus.google.com/\" target=\"_blank\" class=\"text brand light gplus icon icon-gplus\">Google Plus</a>\r\n<a href=\"http://twitter.com/\" target=\"_blank\" class=\"text brand light twitter icon icon-twitter\">Twitter</a>\r\n<a href=\"http://www.facebook.com/\" target=\"_blank\" class=\"text brand light facebook icon icon-facebook\">Facebook</a>\r\n<a href=\"http://www.microsoft.com/\" target=\"_blank\" class=\"text brand light microsoft icon icon-windows\">Microsoft</a>\r\n";
  },"useData":true}));

Handlebars.registerPartial("StartCloudBrand", Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  return "<div style=\"overflow: auto; padding: 10px;\">\r\n    <a class=\"brand logo dropbox\" href=\"http://www.dropbox.com/\" target=\"_blank\">\r\n        <img src=\"/Content/Images/Dropbox/logo-square-100.png\" alt=\"Dropbox\" />\r\n    </a>\r\n    <a class=\"brand logo googledrive\" href=\"http://drive.google.com/drive\" target=\"_blank\">\r\n        <img src=\"/Content/Images/GoogleDrive/logo-square-100.png\" alt=\"Google Drive\" />\r\n    </a>\r\n    <a class=\"brand logo onedrive\" href=\"http://onedrive.live.com/\" target=\"_blank\">\r\n        <img src=\"/Content/Images/OneDrive/logo-square-100.png\" alt=\"OneDrive\" />\r\n    </a>\r\n</div>\r\n<br style=\"clear:both\">\r\n";
  },"useData":true}));

Handlebars.registerPartial("StartFeatures", Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  return "<div class=\"feature-list\">\r\n    <a class=\"feature\" href=\"#\" data-part=\"journal\">\r\n        <i class=\"icon icon-history\" data-i18n=\"journal\"></i>\r\n        <p data-i18n=\"journal-info\"></p>\r\n    </a>\r\n    <a class=\"feature\" href=\"#\" data-part=\"overview\">\r\n        <i class=\"icon icon-grid-large\" data-i18n=\"overview\"></i>\r\n        <p data-i18n=\"overview-info\"></p>\r\n    </a>\r\n    <a class=\"feature\" href=\"#\" data-part=\"category\">\r\n        <i class=\"icon icon-pin\" data-i18n=\"categories\"></i>\r\n        <p data-i18n=\"categories-info\"></p>\r\n    </a>\r\n    <a class=\"feature\" href=\"#\" data-part=\"topic\">\r\n        <i class=\"icon icon-bullseye\" data-i18n=\"topics\"></i>\r\n        <p data-i18n=\"topics-info\"></p>\r\n    </a>\r\n    <a class=\"feature\" href=\"#\" data-part=\"location\">\r\n        <i class=\"icon icon-location\" data-i18n=\"locations\"></i>\r\n        <p data-i18n=\"locations-info\"></p>\r\n    </a>\r\n    <a class=\"feature\" href=\"#\" data-part=\"event\">\r\n        <i class=\"icon icon-rocket\" data-i18n=\"events\"></i>\r\n        <p data-i18n=\"events-info\"></p>\r\n    </a>\r\n    <a class=\"feature\" href=\"#\" data-part=\"story\">\r\n        <i class=\"icon icon-feather\" data-i18n=\"stories\"></i>\r\n        <p data-i18n=\"stories-info\"></p>\r\n    </a>\r\n    <a class=\"feature\" href=\"#\" data-part=\"foto\">\r\n        <i class=\"icon icon-picture\" data-i18n=\"foto\"></i>\r\n        <p data-i18n=\"foto-info\"></p>\r\n    </a>\r\n</div>";
  },"useData":true}));

Handlebars.registerPartial("StartFotosNew", Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  return "<div id=\"start-fotos-new\" class=\"box box-collection\">\r\n    <h1 data-i18n=\"new-fotos\"></h1>\r\n    <div id=\"start-fotos-new-container\" class=\"collection-container\">\r\n        <div id=\"start-fotos-new-wrapper\" class=\"collection-wrapper\" data-page=\"0\">\r\n            <div id=\"fotos-new-more\" class=\"cubicle\">\r\n                <div href=\"#\" onclick=\"showFotosNew(); return false;\" class=\"icon icon-dot-3\" style=\"cursor: pointer;\">\r\n                    <img src=\"/Content/Images/dummy200.png\" />\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>";
  },"useData":true}));

Handlebars.registerPartial("StartFotosTop", Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  return "<div id=\"start-fotos-top\" class=\"box box-collection\">\r\n    <h1 data-i18n=\"top-rated-fotos\"></h1>\r\n    <div id=\"start-fotos-top-container\" class=\"collection-container\">\r\n        <div id=\"start-fotos-top-wrapper\" class=\"collection-wrapper\" data-page=\"0\">\r\n            <div id=\"fotos-top-more\" class=\"cubicle\">\r\n                <div href=\"#\" onclick=\"showFotosTop(); return false;\" class=\"icon icon-dot-3\" style=\"cursor: pointer;\">\r\n                    <img src=\"/Content/Images/dummy200.png\" />\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>";
  },"useData":true}));

Handlebars.registerPartial("StartIntroButtons", Handlebars.template({"1":function(depth0,helpers,partials,data) {
  return "    <p data-i18n=\"have-invitecode\"\r\n       class=\"subtitle\"\r\n       style=\"margin-top: 20px;\">Du hast einen Einladungs-Code?</p>\r\n    <button class=\"bold\"\r\n            style=\"font-size: 1.3rem;\"\r\n            onclick=\"openSidebarBay('register');\"\r\n            data-i18n=\"create-your-fotosteam\"></button>\r\n    <p id=\"tweet-invite-code\"\r\n       data-i18n=\"dont-have-invitecode\"\r\n       class=\"subtitle\"\r\n       style=\"margin-top: 10px;\"></p>\r\n";
  },"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, buffer = "<div id=\"start-intro-buttons\"\r\n     style=\"margin-top:30px;\">\r\n\r\n    <button class=\"shine bold\"\r\n            style=\"font-size: 1.3rem;\"\r\n            onclick=\"showHowTo(this);\"\r\n            data-i18n=\"how-to\"></button>\r\n\r\n";
  stack1 = helpers.unless.call(depth0, (depth0 != null ? depth0.AuthMember : depth0), {"name":"unless","hash":{},"fn":this.program(1, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer + "</div>\r\n";
},"useData":true}));

Handlebars.registerPartial("StartMember", Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  return "<div id=\"start-member\" class=\"box box-collection\">\r\n    <h1 data-i18n=\"discover-fotosteamer\"></h1>\r\n    <div id=\"start-member-container\" class=\"collection-container\">\r\n        <div id=\"start-member-wrapper\" class=\"collection-wrapper collection-small\"></div>\r\n    </div>\r\n</div>";
  },"useData":true}));

Handlebars.registerPartial("StartScreenshot", Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  return "<div id=\"start-intro-screenshot\" class=\"csslider\">\r\n    <input type=\"radio\" name=\"slides\" id=\"slide_journal\" checked />\r\n    <input type=\"radio\" name=\"slides\" id=\"slide_overview\" />\r\n    <input type=\"radio\" name=\"slides\" id=\"slide_foto\" />\r\n    <input type=\"radio\" name=\"slides\" id=\"slide_category\" />\r\n    <input type=\"radio\" name=\"slides\" id=\"slide_topic\" />\r\n    <input type=\"radio\" name=\"slides\" id=\"slide_location\" />\r\n    <input type=\"radio\" name=\"slides\" id=\"slide_event\" />\r\n    <input type=\"radio\" name=\"slides\" id=\"slide_story\" />\r\n    <ul>\r\n        <li><img src=\"/Content/Images/FOTOSTEAM-Journal-500.jpg\" alt=\"\" /></li>\r\n        <li><img src=\"/Content/Images/FOTOSTEAM-Overview-500.jpg\" alt=\"\" /></li>\r\n        <li><img src=\"/Content/Images/FOTOSTEAM-Foto-500.jpg\" alt=\"\" /></li>\r\n        <li><img src=\"/Content/Images/FOTOSTEAM-Category-500.jpg\" alt=\"\" /></li>\r\n        <li><img src=\"/Content/Images/FOTOSTEAM-Topic-500.jpg\" alt=\"\" /></li>\r\n        <li><img src=\"/Content/Images/FOTOSTEAM-Location-500.jpg\" alt=\"\" /></li>\r\n        <li><img src=\"/Content/Images/FOTOSTEAM-Event-500.jpg\" alt=\"\" /></li>\r\n        <li><img src=\"/Content/Images/FOTOSTEAM-Story-500.jpg\" alt=\"\" /></li>\r\n    </ul>\r\n</div>\r\n";
  },"useData":true}));

this["fsTemplates"]["tAbout"] = Handlebars.template({"1":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "                <a class=\"text brand icon icon-mail\"\r\n                   href=\"mailto:"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Email : stack1), depth0))
    + "\" target=\"_blank\">Email</a>\r\n";
},"3":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                <a class=\"text brand icon icon-"
    + escapeExpression(((helper = (helper = helpers.TypeClass || (depth0 != null ? depth0.TypeClass : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TypeClass","hash":{},"data":data}) : helper)))
    + "\"\r\n                   href=\""
    + escapeExpression(((helper = (helper = helpers.Url || (depth0 != null ? depth0.Url : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Url","hash":{},"data":data}) : helper)))
    + "\" target=\"_blank\">"
    + escapeExpression(((helper = (helper = helpers.TypeCaption || (depth0 != null ? depth0.TypeCaption : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TypeCaption","hash":{},"data":data}) : helper)))
    + "</a>\r\n";
},"5":function(depth0,helpers,partials,data) {
  var stack1, helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression, lambda=this.lambda;
  return "        <a class=\"buddy "
    + escapeExpression(((helper = (helper = helpers.MutualClass || (depth0 != null ? depth0.MutualClass : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"MutualClass","hash":{},"data":data}) : helper)))
    + "\" style=\"background-image: url('"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Avatar100Url : stack1), depth0))
    + "');\"\r\n           href=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.SteamPath : stack1), depth0))
    + "/about\"></a>\r\n";
},"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, helper, options, lambda=this.lambda, escapeExpression=this.escapeExpression, blockHelperMissing=helpers.blockHelperMissing, functionType="function", helperMissing=helpers.helperMissing, buffer = "<div class=\"about-info\">\r\n    <div id=\"about-map\"></div>\r\n    <img src=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Avatar200Url : stack1), depth0))
    + "\" />\r\n    <div id=\"about-fotos\">\r\n        <div id=\"about-fotos-count\">"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Fotos : stack1)) != null ? stack1.length : stack1), depth0))
    + " <span data-i18n=\"photos\">Fotos</span></div>\r\n        <a class=\"about-fotos-link\" id=\"about-fotos-journal\"\r\n           href=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.SteamPath : stack1), depth0))
    + "/journal\" data-i18n=\"journal\">Journal</a>\r\n        <a class=\"about-fotos-link\" id=\"about-fotos-overview\"\r\n           href=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.SteamPath : stack1), depth0))
    + "/overview\" data-i18n=\"overview\">Übersicht</a>\r\n    </div>\r\n</div>\r\n<div class=\"about-details page-details\">\r\n\r\n    <div class=\"details-panel\" id=\"about-meta\">\r\n\r\n        <div id=\"about-meta-buddy\"></div>\r\n\r\n        <div id=\"about-meta-info\">\r\n            <h2>"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Motto : stack1), depth0))
    + "</h2>\r\n            <p>"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Description : stack1), depth0))
    + "</p>\r\n\r\n            <div>\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Options : stack1)) != null ? stack1.DisplayEmailAddress : stack1), {"name":"if","hash":{},"fn":this.program(1, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  stack1 = blockHelperMissing.call(depth0, lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.SocialMedias : stack1), depth0), {"name":"Member.SocialMedias","hash":{},"fn":this.program(3, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "            </div>\r\n\r\n            <div id=\"about-meta-fotos\">\r\n                <h3 id=\"fotos-member-random\" data-i18n=\"random-fotos\"></h3>\r\n                <h3 id=\"fotos-member-toprated\" class=\"link\" data-i18n=\"toprated-fotos\"></h3>\r\n                <div class=\"collection-container\">\r\n                    <div class=\"collection-wrapper\"></div>\r\n                </div>\r\n            </div>\r\n        </div>\r\n\r\n        <div id=\"about-meta-collections\">\r\n            <a href=\"#\" class=\"stories\" data-collection=\"stories\">\r\n                <i class=\"icon icon-feather\"></i>\r\n            </a>\r\n            <a href=\"#\" class=\"events\" data-collection=\"events\">\r\n                <i class=\"icon icon-rocket\"></i>\r\n            </a>\r\n            <a href=\"#\" class=\"locations\" data-collection=\"locations\">\r\n                <i class=\"icon icon-location\"></i>\r\n            </a>\r\n            <a href=\"#\" class=\"topics\" data-collection=\"topics\">\r\n                <i class=\"icon icon-bullseye\"></i>\r\n            </a>\r\n            <a href=\"#\" class=\"categories current\" data-collection=\"categories\">\r\n                <i class=\"icon icon-pin\"></i>\r\n            </a>\r\n            <h2 id=\"about-meta-collection-current\"></h2>\r\n            <div id=\"about-meta-collection-items\"></div>\r\n        </div>\r\n    </div>\r\n\r\n    <div class=\"details-panel\" id=\"about-buddies\">\r\n        <h2 data-i18n=\"buddies\"></h2>\r\n";
  stack1 = ((helper = (helper = helpers.BuddiesList || (depth0 != null ? depth0.BuddiesList : depth0)) != null ? helper : helperMissing),(options={"name":"BuddiesList","hash":{},"fn":this.program(5, data),"inverse":this.noop,"data":data}),(typeof helper === functionType ? helper.call(depth0, options) : helper));
  if (!helpers.BuddiesList) { stack1 = blockHelperMissing.call(depth0, stack1, options); }
  if (stack1 != null) { buffer += stack1; }
  return buffer + "    </div>\r\n\r\n</div>\r\n";
},"useData":true});

this["fsTemplates"]["tAboutBuddies"] = Handlebars.template({"1":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers.unless.call(depth0, (depth0 != null ? depth0.IsAuthMember : depth0), {"name":"unless","hash":{},"fn":this.program(2, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"2":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.ShowRequest : depth0), {"name":"if","hash":{},"fn":this.program(3, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.ShowWait : depth0), {"name":"if","hash":{},"fn":this.program(5, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.ShowConfirm : depth0), {"name":"if","hash":{},"fn":this.program(7, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.ShowMutual : depth0), {"name":"if","hash":{},"fn":this.program(9, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"3":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "        <a id=\"buddy-request\" class=\"icon icon-users\" href=\"#\"\r\n           data-i18n=\"buddies-request\"\r\n           data-i18n-args=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.PlainName : stack1), depth0))
    + "\"></a>\r\n";
},"5":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "        <span data-i18n=\"buddies-wait\"\r\n              data-i18n-args=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.PlainName : stack1), depth0))
    + "\"></span>\r\n";
},"7":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "        <a id=\"buddy-confirm\" class=\"icon icon-users\" href=\"#\"\r\n           data-i18n=\"buddies-confirm\"\r\n           data-i18n-args=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.PlainName : stack1), depth0))
    + "\"></a>\r\n";
},"9":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "        <span data-i18n=\"buddies-mutual\"\r\n              data-i18n-args=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.PlainName : stack1), depth0))
    + "\"></span>\r\n";
},"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, buffer = "<div>\r\n";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.AuthMember : depth0), {"name":"if","hash":{},"fn":this.program(1, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer + "</div>";
},"useData":true});

this["fsTemplates"]["tCC0Overview"] = Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "<div id=\"cc0-fotos-fotos\" class=\"box box-collection\">\r\n    <h1 data-i18n=\"license-free-photos\"></h1>\r\n    <input type=\"search\" data-i18n-placeholder=\"search\" class=\"info edit\" style=\"clear: none; width: 200px; clear: none; float: left;\" id=\"searchTerm\" />\r\n    <a href=\"#\" data-i18n-title=\"search\" id=\"SearchCC0Fotos\">\r\n        <i class=\"foto-btn btn-info icon icon-search\" style=\"margin-left: -20px;font-size: 1.2em; float: left;\"></i>\r\n    </a> \r\n    <div id=\"cc0-fotos-top-container\" class=\"collection-container\" style=\"clear:both\">\r\n        <div id=\"cc0-fotos-top-wrapper\" class=\"collection-wrapper\" data-page=\"0\">\r\n          "
    + escapeExpression(((helper = (helper = helpers.OverviewGrid || (depth0 != null ? depth0.OverviewGrid : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"OverviewGrid","hash":{},"data":data}) : helper)))
    + "\r\n              <!--<div id=\"fotos-top-more\" class=\"cubicle\">\r\n                <div href=\"#\" onclick=\"showNextFotos(); return false;\" class=\"icon icon-dot-3\" style=\"cursor: pointer;\">\r\n                    <img src=\"/Content/Images/dummy200.png\" />\r\n                </div>\r\n            </div>-->\r\n        </div>\r\n    </div>\r\n</div>";
},"useData":true});

this["fsTemplates"]["tCommentList"] = Handlebars.template({"1":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = this.invokePartial(partials.Comment, '        ', 'Comment', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, buffer = "<h3 data-i18n=\"comments\">Kommentare</h3>\r\n<div class=\"comment-wrapper panel-scroll\">\r\n";
  stack1 = helpers.each.call(depth0, (depth0 != null ? depth0.Comments : depth0), {"name":"each","hash":{},"fn":this.program(1, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer + "</div>\r\n";
},"usePartial":true,"useData":true});

this["fsTemplates"]["tCommentNew"] = Handlebars.template({"1":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "<div class=\"new-comment\">\r\n    <div class=\"new-comment-text\" contenteditable=\"true\" role=\"textbox\"><br /></div>\r\n    <div class=\"new-comment-cmds\">\r\n        <button class=\"new-comment-send button small\" onclick=\"sendComment(this, "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Id : stack1), depth0))
    + ");\">\r\n            <i class=\"icon icon-mail\"></i>\r\n            <span data-i18n=\"send\">Senden</span>\r\n        </button>\r\n        <button class=\"new-comment-send button small\" onclick=\"cancelAddComment(this);\">\r\n            <i class=\"icon icon-cancel\"></i>\r\n            <span data-i18n=\"cancel\">Abbrechen</span>\r\n        </button>\r\n    </div>\r\n</div>\r\n";
},"3":function(depth0,helpers,partials,data) {
  return "    <p data-i18n=\"your-comment-login-info\"></p>\r\n";
  },"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression, buffer = "<h3 style=\"margin-top: 30px;\">\r\n    <span data-i18n=\"your-comment\">Dein Kommentar</span>, "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.AuthMember : depth0)) != null ? stack1.PlainName : stack1), depth0))
    + "\r\n</h3>\r\n\r\n";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.AuthMember : depth0), {"name":"if","hash":{},"fn":this.program(1, data),"inverse":this.program(3, data),"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"useData":true});

this["fsTemplates"]["tCubicle"] = Handlebars.template({"1":function(depth0,helpers,partials,data) {
  return "        <i class=\"icon icon-star\" i18n-title=\"new\"></i>\r\n";
  },"3":function(depth0,helpers,partials,data) {
  return "        <i class=\"icon icon-lock\" i18n-title=\"private\"></i>\r\n";
  },"5":function(depth0,helpers,partials,data) {
  return "        <i class=\"icon icon-feather\" i18n-title=\"story-only\"></i>\r\n";
  },"7":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "        <a class=\"btn-show icon cubicle-link icon-progress-2\" \r\n            href=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Url1920 : stack1), depth0))
    + "\"\r\n            data-i18n-title=\"full-screen\"\r\n            detailUrl=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Url : stack1), depth0))
    + "\"\r\n            plainName=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.PlainName : stack1), depth0))
    + "\"\r\n            avatarUrl=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Avatar100Url : stack1), depth0))
    + "\"\r\n            alias=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Alias : stack1), depth0))
    + "\">\r\n";
},"9":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "        <a class=\"icon cubicle-link icon-progress-2\" href=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Url : stack1), depth0))
    + "\">\r\n";
},"11":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "            <div class=\"details\">\r\n                <a href=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.SteamPath : stack1), depth0))
    + "/about\">"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.PlainName : stack1), depth0))
    + "</a>\r\n            </div>\r\n";
},"13":function(depth0,helpers,partials,data) {
  return "            <div class=\"selectortrigger\">\r\n                <a href=\"#\"><i class=\"icon icon-ok-circled\"></i></a>\r\n            </div>\r\n";
  },"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression, buffer = "<div class=\"cubicle\" id=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Name : stack1), depth0))
    + "\" data-url=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Url : stack1), depth0))
    + "\">\r\n\r\n    <div class=\"info-icons\">\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.IsNew : stack1), {"name":"if","hash":{},"fn":this.program(1, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "        \r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.IsPrivate : stack1), {"name":"if","hash":{},"fn":this.program(3, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.IsForStoryOnly : stack1), {"name":"if","hash":{},"fn":this.program(5, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "    </div>\r\n\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Url1920 : stack1), {"name":"if","hash":{},"fn":this.program(7, data),"inverse":this.program(9, data),"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "            <img src=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.DummyUrl : stack1), depth0))
    + "\" data-id=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Id : stack1), depth0))
    + "\" style=\"background-image: url("
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.CubicleUrl : stack1), depth0))
    + ")\" />\r\n\r\n            <div class=\"title\">\r\n                <a href=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Url : stack1), depth0))
    + "\">"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Caption : stack1), depth0))
    + "</a>\r\n            </div>\r\n\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.SteamPath : stack1), {"name":"if","hash":{},"fn":this.program(11, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.SelectorTrigger : depth0), {"name":"if","hash":{},"fn":this.program(13, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer + "\r\n        </a>\r\n\r\n</div>\r\n";
},"useData":true});

this["fsTemplates"]["tDashboard"] = Handlebars.template({"1":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "        <a id=\"details-command-sync\" href=\"#\" onclick=\"syncPhotos(); return false;\" data-i18n=\"provider-sync\"><i class=\"icon icon-image icon-"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.StorageProviderKey : stack1), depth0))
    + "\"></i>Synch</a>\r\n";
},"3":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                        <div class=\"field input inline-icon\">\r\n                            <i class=\"text brand icon icon-"
    + escapeExpression(((helper = (helper = helpers.TypeClass || (depth0 != null ? depth0.TypeClass : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TypeClass","hash":{},"data":data}) : helper)))
    + " inline\"></i>\r\n                            <input id=\"sm-"
    + escapeExpression(((helper = (helper = helpers.TypeCaption || (depth0 != null ? depth0.TypeCaption : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TypeCaption","hash":{},"data":data}) : helper)))
    + "\" name=\"sm-"
    + escapeExpression(((helper = (helper = helpers.TypeCaption || (depth0 != null ? depth0.TypeCaption : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TypeCaption","hash":{},"data":data}) : helper)))
    + "\" class=\"social-media\" type=\"text\"\r\n                                    data-type=\""
    + escapeExpression(((helper = (helper = helpers.Type || (depth0 != null ? depth0.Type : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Type","hash":{},"data":data}) : helper)))
    + "\"\r\n                                    data-id=\""
    + escapeExpression(((helper = (helper = helpers.Id || (depth0 != null ? depth0.Id : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Id","hash":{},"data":data}) : helper)))
    + "\"\r\n                                    placeholder=\""
    + escapeExpression(((helper = (helper = helpers.TypeCaption || (depth0 != null ? depth0.TypeCaption : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TypeCaption","hash":{},"data":data}) : helper)))
    + "\" value=\""
    + escapeExpression(((helper = (helper = helpers.Url || (depth0 != null ? depth0.Url : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Url","hash":{},"data":data}) : helper)))
    + "\" />\r\n                            <label for=\"sm-"
    + escapeExpression(((helper = (helper = helpers.TypeCaption || (depth0 != null ? depth0.TypeCaption : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TypeCaption","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.TypeCaption || (depth0 != null ? depth0.TypeCaption : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TypeCaption","hash":{},"data":data}) : helper)))
    + "</label>\r\n                            <span id=\"errorsm-"
    + escapeExpression(((helper = (helper = helpers.TypeCaption || (depth0 != null ? depth0.TypeCaption : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TypeCaption","hash":{},"data":data}) : helper)))
    + "\" class='help-block'></span>\r\n                        </div>\r\n";
},"5":function(depth0,helpers,partials,data) {
  return "                    <div class=\"pagenote missing\" data-i18n=\"invitecodes-pagenote\"></div>\r\n";
  },"7":function(depth0,helpers,partials,data) {
  var stack1, buffer = "                    <ul class=\"square\">\r\n";
  stack1 = helpers.each.call(depth0, ((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.InviteCodes : stack1), {"name":"each","hash":{},"fn":this.program(8, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer + "                    </ul>\r\n";
},"8":function(depth0,helpers,partials,data) {
  var lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "                        <li><code>"
    + escapeExpression(lambda(depth0, depth0))
    + "</code></li>\r\n";
},"10":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.Selected : depth0), {"name":"if","hash":{},"fn":this.program(11, data),"inverse":this.program(13, data),"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"11":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                            <option value=\""
    + escapeExpression(((helper = (helper = helpers.Iso || (depth0 != null ? depth0.Iso : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Iso","hash":{},"data":data}) : helper)))
    + "\" selected=\"selected\">"
    + escapeExpression(((helper = (helper = helpers.Caption || (depth0 != null ? depth0.Caption : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Caption","hash":{},"data":data}) : helper)))
    + "</option>\r\n";
},"13":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                            <option value=\""
    + escapeExpression(((helper = (helper = helpers.Iso || (depth0 != null ? depth0.Iso : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Iso","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.Caption || (depth0 != null ? depth0.Caption : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Caption","hash":{},"data":data}) : helper)))
    + "</option>\r\n";
},"15":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "                        <label data-i18n=\"option-general-use-dropbox-webhook\"></label>\r\n                        <input id=\"UseDropboxWebhook\" type=\"checkbox\" class=\"ios yesno\" "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.OptionAttributes : depth0)) != null ? stack1.UseDropboxWebhook : stack1), depth0))
    + " />\r\n";
},"17":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.Selected : depth0), {"name":"if","hash":{},"fn":this.program(18, data),"inverse":this.program(20, data),"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"18":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                            <option value=\""
    + escapeExpression(((helper = (helper = helpers.Value || (depth0 != null ? depth0.Value : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Value","hash":{},"data":data}) : helper)))
    + "\" selected=\"selected\">"
    + escapeExpression(((helper = (helper = helpers.Caption || (depth0 != null ? depth0.Caption : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Caption","hash":{},"data":data}) : helper)))
    + "</option>\r\n";
},"20":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                            <option value=\""
    + escapeExpression(((helper = (helper = helpers.Value || (depth0 != null ? depth0.Value : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Value","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.Caption || (depth0 != null ? depth0.Caption : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Caption","hash":{},"data":data}) : helper)))
    + "</option>\r\n";
},"22":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                            <a id=\"topic-"
    + escapeExpression(((helper = (helper = helpers.TopicId || (depth0 != null ? depth0.TopicId : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TopicId","hash":{},"data":data}) : helper)))
    + "\" class=\"collection-item collection-topic\"\r\n                               data-id=\""
    + escapeExpression(((helper = (helper = helpers.TopicId || (depth0 != null ? depth0.TopicId : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TopicId","hash":{},"data":data}) : helper)))
    + "\" href=\"#\"><span>"
    + escapeExpression(((helper = (helper = helpers.TopicName || (depth0 != null ? depth0.TopicName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TopicName","hash":{},"data":data}) : helper)))
    + "</span><i>"
    + escapeExpression(((helper = (helper = helpers.TopicPhotoCount || (depth0 != null ? depth0.TopicPhotoCount : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TopicPhotoCount","hash":{},"data":data}) : helper)))
    + "</i></a>\r\n";
},"24":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                            <a id=\"location-"
    + escapeExpression(((helper = (helper = helpers.LocationId || (depth0 != null ? depth0.LocationId : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationId","hash":{},"data":data}) : helper)))
    + "\" class=\"collection-item collection-location\"\r\n                               data-id=\""
    + escapeExpression(((helper = (helper = helpers.LocationId || (depth0 != null ? depth0.LocationId : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationId","hash":{},"data":data}) : helper)))
    + "\" href=\"#\"><span>"
    + escapeExpression(((helper = (helper = helpers.LocationName || (depth0 != null ? depth0.LocationName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationName","hash":{},"data":data}) : helper)))
    + "</span><i>"
    + escapeExpression(((helper = (helper = helpers.LocationPhotoCount || (depth0 != null ? depth0.LocationPhotoCount : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationPhotoCount","hash":{},"data":data}) : helper)))
    + "</i></a>\r\n";
},"26":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                            <a id=\"event-"
    + escapeExpression(((helper = (helper = helpers.EventId || (depth0 != null ? depth0.EventId : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"EventId","hash":{},"data":data}) : helper)))
    + "\" class=\"collection-item collection-event\"\r\n                               data-id=\""
    + escapeExpression(((helper = (helper = helpers.EventId || (depth0 != null ? depth0.EventId : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"EventId","hash":{},"data":data}) : helper)))
    + "\" href=\"#\"><span>"
    + escapeExpression(((helper = (helper = helpers.EventName || (depth0 != null ? depth0.EventName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"EventName","hash":{},"data":data}) : helper)))
    + "</span><i>"
    + escapeExpression(((helper = (helper = helpers.EventPhotoCount || (depth0 != null ? depth0.EventPhotoCount : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"EventPhotoCount","hash":{},"data":data}) : helper)))
    + "</i></a>\r\n";
},"28":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers.unless.call(depth0, (depth0 != null ? depth0.UsesWebHook : depth0), {"name":"unless","hash":{},"fn":this.program(29, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"29":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "                        <div class=\"pagenote hint\" style=\"margin-top: 20px;\"\r\n                             data-i18n=\"newphotos-webhook-pagenote\"\r\n                             data-i18n-args=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.StorageProviderCaption : stack1), depth0))
    + "\"></div>\r\n";
},"31":function(depth0,helpers,partials,data) {
  return "                        <div class=\"devnote\" style=\"margin-top: 20px;\"\r\n                             data-i18n=\"newphotos-devnote\"></div>\r\n";
  },"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, helper, options, lambda=this.lambda, escapeExpression=this.escapeExpression, blockHelperMissing=helpers.blockHelperMissing, functionType="function", helperMissing=helpers.helperMissing, buffer = "<div id=\"dashboard\" class=\"page-details\">\r\n\r\n    <div class=\"details-commands\">\r\n";
  stack1 = helpers.unless.call(depth0, (depth0 != null ? depth0.UsesWebHook : depth0), {"name":"unless","hash":{},"fn":this.program(1, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "        <a href=\"#\" class=\"tooltip-left\" onclick=\"global_auth_member.deleteAccount(); return false;\" data-i18n-title=\"delete-account\"><i class=\"icon icon-trash\"></i></a>\r\n    </div>\r\n\r\n    <div class=\"details-panel\" style=\"padding: 0; background: none;\">\r\n        <div id=\"dashboard-tabs\" class=\"tabulous-wrapper\">\r\n            <ul>\r\n                <li><a id=\"tablink-profile\" href=\"#profile\" data-i18n=\"dashboard-tab-profile\">Profil</a></li>\r\n                <li><a id=\"tablink-options\" href=\"#options\" data-i18n=\"dashboard-tab-options\">Optionen</a></li>\r\n                <li><a id=\"tablink-topics\" href=\"#topics\" data-i18n=\"dashboard-tab-topics\">Themen</a></li>\r\n                <li><a id=\"tablink-locations\" href=\"#locations\" data-i18n=\"dashboard-tab-locations\">Orte</a></li>\r\n                <li><a id=\"tablink-events\" href=\"#events\" data-i18n=\"dashboard-tab-events\">Events</a></li>\r\n                <li><a id=\"tablink-stories\" href=\"#stories\" data-i18n=\"dashboard-tab-stories\">Stories</a></li>\r\n                <li><a id=\"tablink-newphotos\" href=\"#newphotos\" data-i18n=\"dashboard-tab-newphotos\">Neue Fotos</a></li>\r\n            </ul>\r\n\r\n            <div class=\"tabulous-container\">\r\n\r\n            <div id=\"profile\" class=\"tabulous-shelf\">\r\n\r\n                <section class=\"flex\">\r\n                    <div id=\"tab-profile-common\">\r\n                        <form id=\"profile-common-form\">\r\n                            <div class=\"field input\">\r\n                                <input id=\"PlainName\" name=\"PlainName\" type=\"text\"\r\n                                        data-i18n-placeholder=\"your-full-name\" value=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.PlainName : stack1), depth0))
    + "\" />\r\n                                <label for=\"PlainName\" data-i18n=\"your-full-name\">PlainName</label>\r\n                                <span id=\"errorPlainName\" class='help-block'></span>\r\n                            </div>\r\n                            <div class=\"field input\">\r\n                                <input id=\"EMail\" name=\"EMail\" type=\"email\"\r\n                                        data-i18n-placeholder=\"your-mail-address\" value=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Email : stack1), depth0))
    + "\" />\r\n                                <label for=\"EMail\" data-i18n=\"your-mail-address\">EMail</label>\r\n                                <span id=\"errorEMail\" class='help-block'></span>\r\n                            </div>\r\n                            <div class=\"field input\">\r\n                                <textarea id=\"Motto\" data-i18n-placeholder=\"your-motto\">"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Motto : stack1), depth0))
    + "</textarea>\r\n                                <label for=\"Motto\" data-i18n=\"your-motto\">Motto</label>\r\n                            </div>\r\n                            <div class=\"field input\">\r\n                                <textarea id=\"Description\" data-i18n-placeholder=\"your-description\">"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Description : stack1), depth0))
    + "</textarea>\r\n                                <label for=\"Description\" data-i18n=\"your-description\">Beschreibung</label>\r\n                            </div>\r\n                        </form>\r\n                    </div>\r\n                    <div id=\"tab-profile-avatar\">\r\n                        <form id=\"profile-avatar-form\" class=\"dropzone\" action=\"/upload-target\">\r\n                            <div class=\"current\"><img src=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Avatar200Url : stack1), depth0))
    + "\" /></div>\r\n                        </form>\r\n                    </div>\r\n                </section>\r\n                <section>\r\n                    <h2 data-i18n=\"your-header-image\"></h2>\r\n                    <form id=\"profile-header-image-form\" class=\"dropzone\" action=\"/upload-target\">\r\n                        <div class=\"current\"><img src=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Header640Url : stack1), depth0))
    + "\" /></div>\r\n                    </form>\r\n                    <h3 data-i18n=\"your-header-color\" style=\"margin-top: 10px;\"></h3>\r\n                    <div class=\"color-chooser\">\r\n                        <a id=\"header-color-reset\" href=\"#\" data-i18n-title=\"header-reset-color\">\r\n                            <i class=\"icon icon-arrows-cw\"></i>\r\n                        </a>\r\n                        <label id=\"header-color-label\" for=\"Color\">"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.HeaderColorHex : stack1), depth0))
    + "</label>\r\n                        <input id=\"HeaderColor\" type=\"color\" value=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.HeaderColorHex : stack1), depth0))
    + "\" />\r\n                    </div>\r\n                </section>\r\n                <section id=\"profile-map-section\">\r\n                    <h2 class=\"principal\" data-i18n=\"your-location\"></h2>\r\n                    <div class=\"adjunct\">\r\n                        <input id=\"profile-map-address\" type=\"text\" data-i18n-placeholder=\"address\" />\r\n                    </div>\r\n                    <div id=\"profile-map\"></div>\r\n                    <div style=\"text-align: right; margin-top: 5px\">\r\n                        <small style=\"display: inline-block; float: left;     margin-right: 10px;width: 50%;text-align: left;line-height: 14px;\" data-i18n=\"homelocation-info\"></small>\r\n                        <button id=\"profile-map-remove\" class=\"small secondary\" data-i18n=\"remove-homelocation\">Standort entfernen</button>\r\n                        <button id=\"profile-map-save\" class=\"small\" data-i18n=\"save\">Speichern</button>\r\n                    </div>\r\n                </section>\r\n                <section>\r\n                    <h2 data-i18n=\"social-media\"></h2>\r\n                    <form id=\"profile-social-media-form\">\r\n";
  stack1 = blockHelperMissing.call(depth0, lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.SocialMediaFullList : stack1), depth0), {"name":"Member.SocialMediaFullList","hash":{},"fn":this.program(3, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "                    </form>\r\n                </section>\r\n                <section>\r\n                    <h2 data-i18n=\"invitecodes\"></h2>\r\n";
  stack1 = helpers.unless.call(depth0, ((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.InviteCodes : stack1), {"name":"unless","hash":{},"fn":this.program(5, data),"inverse":this.program(7, data),"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "                </section>\r\n            </div>\r\n\r\n            <div id=\"options\" class=\"tabulous-shelf\">\r\n                <div class=\"flex options\">\r\n\r\n                    <section class=\"collection-item-list\">\r\n                        <h2 data-i18n=\"option-general\"></h2>\r\n\r\n                        <label data-i18n=\"option-general-language\"></label>\r\n                        <select id=\"Language\">\r\n";
  stack1 = ((helper = (helper = helpers.OptionLanguages || (depth0 != null ? depth0.OptionLanguages : depth0)) != null ? helper : helperMissing),(options={"name":"OptionLanguages","hash":{},"fn":this.program(10, data),"inverse":this.noop,"data":data}),(typeof helper === functionType ? helper.call(depth0, options) : helper));
  if (!helpers.OptionLanguages) { stack1 = blockHelperMissing.call(depth0, stack1, options); }
  if (stack1 != null) { buffer += stack1; }
  buffer += "                        </select>\r\n                        \r\n";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.UsesDropbox : depth0), {"name":"if","hash":{},"fn":this.program(15, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n                        <label data-i18n=\"option-overwrite-existing-photo\"></label>\r\n                        <input id=\"OverwriteExistingPhoto\" type=\"checkbox\" class=\"ios yesno\" "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.OptionAttributes : depth0)) != null ? stack1.OverwriteExistingPhoto : stack1), depth0))
    + " />\r\n\r\n                        <label data-i18n=\"option-general-allow-comments\"></label>\r\n                        <input id=\"AllowComments\" type=\"checkbox\" class=\"ios yesno\" "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.OptionAttributes : depth0)) != null ? stack1.AllowComments : stack1), depth0))
    + " />\r\n\r\n                        <label data-i18n=\"option-general-allow-rating\"></label>\r\n                        <input id=\"AllowRating\" type=\"checkbox\" class=\"ios yesno\" "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.OptionAttributes : depth0)) != null ? stack1.AllowRating : stack1), depth0))
    + " />\r\n\r\n                        <label data-i18n=\"option-general-allow-sharing\"></label>\r\n                        <input id=\"AllowSharing\" type=\"checkbox\" class=\"ios yesno\" "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.OptionAttributes : depth0)) != null ? stack1.AllowSharing : stack1), depth0))
    + " />\r\n\r\n                        <label data-i18n=\"option-general-display-email-address\"></label>\r\n                        <input id=\"DisplayEmailAddress\" type=\"checkbox\" class=\"ios yesno\" "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.OptionAttributes : depth0)) != null ? stack1.DisplayEmailAddress : stack1), depth0))
    + " />\r\n\r\n                    </section>\r\n\r\n                    <section class=\"collection-item-list\">\r\n                        <h2 data-i18n=\"option-notifications\"></h2>\r\n\r\n                        <p data-i18n=\"option-notifications-on\"></p>\r\n\r\n                        <label data-i18n=\"option-notifications-on-comment\"></label>\r\n                        <input id=\"NotifyByEmailOnComment\" type=\"checkbox\" class=\"ios yesno\" "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.OptionAttributes : depth0)) != null ? stack1.NotifyByEmailOnComment : stack1), depth0))
    + " />\r\n\r\n                        <label data-i18n=\"option-notifications-on-rating\"></label>\r\n                        <input id=\"NotifyByEmailOnRating\" type=\"checkbox\" class=\"ios yesno\" "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.OptionAttributes : depth0)) != null ? stack1.NotifyByEmailOnRating : stack1), depth0))
    + " />\r\n\r\n                        <label data-i18n=\"option-notifications-on-buddy-add\"></label>\r\n                        <input id=\"NotifyByEmailOnBuddyAdd\" type=\"checkbox\" class=\"ios yesno\" "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.OptionAttributes : depth0)) != null ? stack1.NotifyByEmailOnBuddyAdd : stack1), depth0))
    + " />\r\n\r\n                        <label data-i18n=\"option-notifications-on-buddy-confirmation\"></label>\r\n                        <input id=\"NotifyByEmailOnBuddyConfirmation\" type=\"checkbox\" class=\"ios yesno\" "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.OptionAttributes : depth0)) != null ? stack1.NotifyByEmailOnBuddyConfirmation : stack1), depth0))
    + " />\r\n\r\n                        <label data-i18n=\"option-notifications-on-buddy-added-photo\"></label>\r\n                        <input id=\"NotifyByEmailOnBuddyAddedPhoto\" type=\"checkbox\" class=\"ios yesno\" "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.OptionAttributes : depth0)) != null ? stack1.NotifyByEmailOnBuddyAddedPhoto : stack1), depth0))
    + " />\r\n\r\n                    </section>\r\n\r\n                    <section class=\"collection-item-list\">\r\n                        <h2 data-i18n=\"option-defaults\"></h2>\r\n\r\n                        <p data-i18n=\"option-defaults-info\"></p>\r\n\r\n                        <label data-i18n=\"option-default-is-private\">Status</label>\r\n                        <input id=\"DefaultIsPrivate\" type=\"checkbox\" class=\"ios pubpriv\" "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.OptionAttributes : depth0)) != null ? stack1.DefaultIsPrivate : stack1), depth0))
    + " />\r\n\r\n                        <label data-i18n=\"option-default-license\">Lizenz</label>\r\n                        <select id=\"DefaultLicense\">\r\n";
  stack1 = ((helper = (helper = helpers.OptionLicenses || (depth0 != null ? depth0.OptionLicenses : depth0)) != null ? helper : helperMissing),(options={"name":"OptionLicenses","hash":{},"fn":this.program(17, data),"inverse":this.noop,"data":data}),(typeof helper === functionType ? helper.call(depth0, options) : helper));
  if (!helpers.OptionLicenses) { stack1 = blockHelperMissing.call(depth0, stack1, options); }
  if (stack1 != null) { buffer += stack1; }
  buffer += "                        </select>\r\n\r\n                        <label data-i18n=\"option-default-allow-fullsize-download\"></label>\r\n                        <input id=\"DefaultAllowFullSizeDownload\" type=\"checkbox\" class=\"ios allowdeny\" "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.OptionAttributes : depth0)) != null ? stack1.DefaultAllowFullSizeDownload : stack1), depth0))
    + " />\r\n\r\n                        <label data-i18n=\"option-default-allow-commenting\"></label>\r\n                        <input id=\"DefaultAllowCommenting\" type=\"checkbox\" class=\"ios allowdeny\" "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.OptionAttributes : depth0)) != null ? stack1.DefaultAllowCommenting : stack1), depth0))
    + " />\r\n\r\n                        <label data-i18n=\"option-default-allow-rating\"></label>\r\n                        <input id=\"DefaultAllowRating\" type=\"checkbox\" class=\"ios allowdeny\" "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.OptionAttributes : depth0)) != null ? stack1.DefaultAllowRating : stack1), depth0))
    + " />\r\n\r\n                        <label data-i18n=\"option-default-allow-sharing\"></label>\r\n                        <input id=\"DefaultAllowSharing\" type=\"checkbox\" class=\"ios allowdeny\" "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.OptionAttributes : depth0)) != null ? stack1.DefaultAllowSharing : stack1), depth0))
    + " />\r\n\r\n                        <label data-i18n=\"option-default-allow-promoting\"></label>\r\n                        <input id=\"DefaultAllowPromoting\" type=\"checkbox\" class=\"ios allowdeny\" "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.OptionAttributes : depth0)) != null ? stack1.DefaultAllowPromoting : stack1), depth0))
    + " />\r\n\r\n                    </section>\r\n                </div>\r\n            </div>\r\n\r\n            <div id=\"topics\" class=\"tabulous-shelf\">\r\n                <section class=\"flex\">\r\n                    <div id=\"topic-list\" class=\"collection-item-list\">\r\n                        <p data-i18n=\"list-intro-topic\"></p>\r\n                        <div class=\"collection-items\">\r\n";
  stack1 = blockHelperMissing.call(depth0, lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Topics : stack1), depth0), {"name":"Member.Topics","hash":{},"fn":this.program(22, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "                        </div>\r\n                    </div>\r\n                    <div id=\"topic-edit\" class=\"collection-item-edit\">\r\n                        <p data-i18n=\"edit-intro-topic\"></p>\r\n                        <div style=\"height: 100%;\"></div>\r\n                    </div>\r\n                </section>\r\n            </div>\r\n\r\n            <div id=\"locations\" class=\"tabulous-shelf\">\r\n                <section class=\"flex\">\r\n                    <div id=\"location-list\" class=\"collection-item-list\">\r\n                        <p data-i18n=\"list-intro-location\"></p>\r\n                        <div class=\"collection-items\">\r\n";
  stack1 = blockHelperMissing.call(depth0, lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.LocationList : stack1), depth0), {"name":"Member.LocationList","hash":{},"fn":this.program(24, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "                        </div>\r\n                    </div>\r\n                    <div id=\"location-edit\" class=\"collection-item-edit\">\r\n                        <p data-i18n=\"edit-intro-location\"></p>\r\n                        <div style=\"height: 100%;\"></div>\r\n                    </div>\r\n                </section>\r\n            </div>\r\n\r\n            <div id=\"events\" class=\"tabulous-shelf\">\r\n                <section class=\"flex\">\r\n                    <div id=\"event-list\" class=\"collection-item-list\">\r\n                        <p data-i18n=\"list-intro-event\"></p>\r\n                        <div class=\"collection-items\">\r\n";
  stack1 = blockHelperMissing.call(depth0, lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Events : stack1), depth0), {"name":"Member.Events","hash":{},"fn":this.program(26, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "                        </div>\r\n                    </div>\r\n                    <div id=\"event-edit\" class=\"collection-item-edit\">\r\n                        <p data-i18n=\"edit-intro-event\"></p>\r\n                        <div style=\"height: 100%;\"></div>\r\n                    </div>\r\n                </section>\r\n            </div>\r\n\r\n            <div id=\"stories\" class=\"tabulous-shelf\">\r\n                <div class=\"devnote\" data-i18n=\"stories-devnote\" style=\"margin-top: 20px;\"></div>\r\n            </div>\r\n\r\n            <div id=\"newphotos\" class=\"tabulous-shelf\">\r\n                <section class=\"flex new-photos\">\r\n                    <div id=\"new-foto-list\" class=\"collection-item-list\">\r\n                        <p id=\"newphotos-info-none\" data-i18n=\"newphotos-info-none\"></p>\r\n                        <p id=\"newphotos-info\" data-i18n=\"newphotos-info\" style=\"display:none;\"></p>\r\n                        <div id=\"new-photo-wrapper\" class=\"collection-wrapper\"></div>\r\n                    </div>\r\n                    <div id=\"new-foto-upload\" class=\"collection-item-edit\">\r\n                        <br style=\"clear:both;\" />\r\n\r\n                        <h3 data-i18n=\"newphotos-upload\"></h3>\r\n                        <form id=\"foto-upload-form\" class=\"dropzone\" action=\"/upload-target\">\r\n                            <div class=\"current\"></div>\r\n                        </form>\r\n\r\n";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.HasWebHook : depth0), {"name":"if","hash":{},"fn":this.program(28, data),"inverse":this.program(31, data),"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer + "\r\n                    </div>\r\n                </section>\r\n\r\n            </div>\r\n\r\n        </div>\r\n\r\n    </div>\r\n</div>\r\n</div>";
},"useData":true});

this["fsTemplates"]["tEditEvent"] = Handlebars.template({"1":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.Selected : depth0), {"name":"if","hash":{},"fn":this.program(2, data),"inverse":this.program(4, data),"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"2":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "            <option value=\""
    + escapeExpression(((helper = (helper = helpers.LocationId || (depth0 != null ? depth0.LocationId : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationId","hash":{},"data":data}) : helper)))
    + "\" selected=\"selected\">"
    + escapeExpression(((helper = (helper = helpers.LocationName || (depth0 != null ? depth0.LocationName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationName","hash":{},"data":data}) : helper)))
    + "</option>\r\n";
},"4":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "            <option value=\""
    + escapeExpression(((helper = (helper = helpers.LocationId || (depth0 != null ? depth0.LocationId : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationId","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.LocationName || (depth0 != null ? depth0.LocationName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationName","hash":{},"data":data}) : helper)))
    + "</option>\r\n";
},"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, helper, options, lambda=this.lambda, escapeExpression=this.escapeExpression, functionType="function", helperMissing=helpers.helperMissing, blockHelperMissing=helpers.blockHelperMissing, buffer = "<form id=\"event-edit-form\">\r\n    <div class=\"field input\">\r\n        <input id=\"EventName\" name=\"EventName\" type=\"text\"\r\n               data-i18n-placeholder=\"Event-name\" value=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Event : depth0)) != null ? stack1.EventName : stack1), depth0))
    + "\" />\r\n        <label for=\"EventName\" data-i18n=\"event-name\">Name</label>\r\n        <span id=\"errorEventName\" class='help-block'></span>\r\n        <small class=\"edit-note\" data-i18n=\"event-name-edit-note\"></small>\r\n    </div>\r\n    <div class=\"field input\">\r\n        <input id=\"EventUrl\" name=\"EventUrl\" type=\"text\" disabled=\"disabled\"\r\n               data-i18n-placeholder=\"event-url\" value=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Event : depth0)) != null ? stack1.EventUrl : stack1), depth0))
    + "\" />\r\n        <label for=\"EventUrl\" data-i18n=\"event-url\">Url</label>\r\n    </div>    \r\n    <div class=\"field input\">\r\n        <input id=\"Date\" name=\"Date\" type=\"text\"\r\n               data-i18n-placeholder=\"event-date\" value=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Event : depth0)) != null ? stack1.EventDate : stack1), depth0))
    + "\" />\r\n        <label for=\"Date\" data-i18n=\"event-date\">Datum</label>\r\n        <span id=\"errorDate\" class='help-block'></span>\r\n    </div>\r\n    <div class=\"field input\">\r\n        <input id=\"DateTo\" name=\"DateTo\" type=\"text\"\r\n               data-i18n-placeholder=\"event-date-to\" value=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Event : depth0)) != null ? stack1.EventDateTo : stack1), depth0))
    + "\" />\r\n        <label for=\"DateTo\" data-i18n=\"event-date-to\">Datum bis</label>\r\n        <span id=\"errorDateTo\" class='help-block'></span>\r\n    </div>    \r\n    <div class=\"field\">\r\n        <label for=\"EventLocationId\" class=\"input-label\" data-i18n=\"location\"></label>\r\n        <select id=\"EventLocationId\" \r\n                data-i18n-placeholder=\"event-location\" \r\n                data-placeholder=\""
    + escapeExpression(((helper = (helper = helpers.ChoosePlaceholder || (depth0 != null ? depth0.ChoosePlaceholder : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"ChoosePlaceholder","hash":{},"data":data}) : helper)))
    + "\">\r\n            <option value=\"0\" data-i18n=\"no-location\"></option>\r\n";
  stack1 = ((helper = (helper = helpers.Locations || (depth0 != null ? depth0.Locations : depth0)) != null ? helper : helperMissing),(options={"name":"Locations","hash":{},"fn":this.program(1, data),"inverse":this.noop,"data":data}),(typeof helper === functionType ? helper.call(depth0, options) : helper));
  if (!helpers.Locations) { stack1 = blockHelperMissing.call(depth0, stack1, options); }
  if (stack1 != null) { buffer += stack1; }
  return buffer + "        </select>\r\n    </div>\r\n    <div class=\"field input\">\r\n        <textarea id=\"EventDescription\" data-i18n-placeholder=\"event-description\">"
    + escapeExpression(((helper = (helper = helpers.EventDescription || (depth0 != null ? depth0.EventDescription : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"EventDescription","hash":{},"data":data}) : helper)))
    + "</textarea>\r\n        <label for=\"EventDescription\" data-i18n=\"event-description\">Description</label>\r\n    </div>\r\n    <button class=\"small ghost\" style=\"float:right;\" data-i18n=\"event-delete\"></button>\r\n    <input id=\"EventDate\" name=\"EventDate\" type=\"hidden\" data-i18n-placeholder=\"event-date\" />\r\n    <input id=\"EventDateTo\" name=\"EventDateTo\" type=\"hidden\" data-i18n-placeholder=\"event-date-to\" />\r\n</form>";
},"useData":true});

this["fsTemplates"]["tEditLocation"] = Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, helper, lambda=this.lambda, escapeExpression=this.escapeExpression, functionType="function", helperMissing=helpers.helperMissing;
  return "<form id=\"location-edit-form\">\r\n    <div id=\"location-map\"></div>\r\n    <div class=\"field input\">\r\n        <input id=\"LocationName\" name=\"LocationName\" type=\"text\"\r\n               data-i18n-placeholder=\"location-name\" value=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Location : depth0)) != null ? stack1.LocationName : stack1), depth0))
    + "\" />\r\n        <label for=\"LocationName\" data-i18n=\"location-name\">Name</label>\r\n        <span id=\"errorLocationName\" class='help-block'></span>\r\n        <small class=\"edit-note\" data-i18n=\"location-name-edit-note\"></small>\r\n    </div>\r\n    <div class=\"field input\">\r\n        <input id=\"LocationUrl\" name=\"LocationUrl\" type=\"text\" disabled=\"disabled\"\r\n               data-i18n-placeholder=\"location-url\" value=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Location : depth0)) != null ? stack1.LocationUrl : stack1), depth0))
    + "\" />\r\n        <label for=\"LocationUrl\" data-i18n=\"location-url\">Url</label>\r\n    </div>\r\n    <div class=\"field input\">\r\n        <input id=\"LocationLatitude\" name=\"LocationLatitude\" type=\"text\"\r\n               data-i18n-placeholder=\"location-latitude\" value=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Location : depth0)) != null ? stack1.LocationLatitude : stack1), depth0))
    + "\" />\r\n        <label for=\"LocationLatitude\" data-i18n=\"location-latitude\">Latitude</label>\r\n    </div>\r\n    <div class=\"field input\">\r\n        <input id=\"LocationLongitude\" name=\"LocationLongitude\" type=\"text\"\r\n               data-i18n-placeholder=\"location-longitude\" value=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Location : depth0)) != null ? stack1.LocationLongitude : stack1), depth0))
    + "\" />\r\n        <label for=\"LocationLongitude\" data-i18n=\"location-longitude\">Longitude</label>\r\n    </div>\r\n    <div class=\"field input\">\r\n        <input id=\"LocationStreet\" name=\"LocationStreet\" type=\"text\"\r\n               data-i18n-placeholder=\"location-street\" value=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Location : depth0)) != null ? stack1.LocationStreet : stack1), depth0))
    + "\" />\r\n        <label for=\"LocationStreet\" data-i18n=\"location-street\">Street</label>\r\n    </div>\r\n    <div class=\"field input\">\r\n        <input id=\"LocationCity\" name=\"LocationCity\" type=\"text\"\r\n               data-i18n-placeholder=\"location-city\" value=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Location : depth0)) != null ? stack1.LocationCity : stack1), depth0))
    + "\" />\r\n        <label for=\"LocationCity\" data-i18n=\"location-city\">City</label>\r\n        <!--<span id=\"errorLocationCity\" class='help-block'></span>-->\r\n    </div>\r\n    <div class=\"field input\">\r\n        <input id=\"LocationCounty\" name=\"LocationCounty\" type=\"text\"\r\n               data-i18n-placeholder=\"location-county\" value=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Location : depth0)) != null ? stack1.LocationCounty : stack1), depth0))
    + "\" />\r\n        <label for=\"LocationCounty\" data-i18n=\"location-county\">County</label>\r\n        <!--<span id=\"errorLocationCounty\" class='help-block'></span>-->\r\n    </div>\r\n    <div class=\"field input\">\r\n        <input id=\"LocationCountry\" name=\"LocationCountry\" type=\"text\"\r\n               data-i18n-placeholder=\"location-country\" value=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Location : depth0)) != null ? stack1.LocationCountry : stack1), depth0))
    + "\" />\r\n        <label for=\"LocationCountry\" data-i18n=\"location-country\">Country</label>\r\n        <span id=\"errorLocationCountry\" class='help-block'></span>\r\n    </div>\r\n    <div class=\"field input\">\r\n        <input id=\"LocationCountryIsoCode\" name=\"LocationCountryIsoCode\" type=\"text\"\r\n                data-i18n-placeholder=\"location-country-iso-code\" value=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Location : depth0)) != null ? stack1.LocationCountryIsoCode : stack1), depth0))
    + "\" />\r\n        <label for=\"LocationCountryIsoCode\" data-i18n=\"location-country-iso-code\">CountryIsoCode</label>\r\n        <span id=\"errorLocationCountryIsoCode\" class='help-block'></span>\r\n    </div>\r\n    \r\n    <div style=\"overflow: auto;\">\r\n        <button id=\"DeleteLocation\" class=\"small ghost\" style=\"float:right;\" data-i18n=\"location-delete\"></button>\r\n        <button id=\"MergeLocation\" class=\"small ghost\" style=\"float:right; margin-right: 10px;\" data-i18n=\"location-merge\"></button>\r\n    </div>\r\n\r\n    <div id=\"merge-location-wrapper\" class=\"instant-input\" style=\"display: none;\">\r\n        <label data-i18n=\"location-merge-info\">Merge this location with:</label>\r\n        <select id=\"MergeLocationSelect\"\r\n                data-i18n-placeholder=\"location-merge\"\r\n                data-placeholder=\""
    + escapeExpression(((helper = (helper = helpers.ChoosePlaceholder || (depth0 != null ? depth0.ChoosePlaceholder : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"ChoosePlaceholder","hash":{},"data":data}) : helper)))
    + "\"></select>\r\n        <div class=\"instant-cmds\">\r\n            <button id=\"MergeLocationCancel\" class=\"small ghost\" style=\"float:right;\"\r\n                    data-i18n=\"cancel\">\r\n                Cancel\r\n            </button>\r\n            <button id=\"MergeLocationOk\" class=\"small\" style=\"float: right; margin-right: 10px;\"\r\n                    data-i18n=\"location-merge-command\">\r\n                Merge\r\n            </button>\r\n        </div>\r\n    </div>\r\n</form>\r\n";
},"useData":true});

this["fsTemplates"]["tEditTopic"] = Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "<form id=\"topic-edit-form\">\r\n    <div class=\"field input\">\r\n        <input id=\"TopicName\" name=\"TopicName\" type=\"text\"\r\n               data-i18n-placeholder=\"topic-name\" value=\""
    + escapeExpression(((helper = (helper = helpers.TopicName || (depth0 != null ? depth0.TopicName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TopicName","hash":{},"data":data}) : helper)))
    + "\" />\r\n        <label for=\"TopicName\" data-i18n=\"topic-name\">Name</label>\r\n        <span id=\"errorTopicName\" class='help-block'></span>\r\n        <small class=\"edit-note\" data-i18n=\"topic-name-edit-note\"></small>\r\n    </div>\r\n    <div class=\"field input\">\r\n        <input id=\"TopicUrl\" name=\"TopicUrl\" type=\"text\" disabled=\"disabled\"\r\n               data-i18n-placeholder=\"topic-url\" value=\""
    + escapeExpression(((helper = (helper = helpers.TopicUrl || (depth0 != null ? depth0.TopicUrl : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TopicUrl","hash":{},"data":data}) : helper)))
    + "\" />\r\n        <label for=\"TopicUrl\" data-i18n=\"topic-url\">Url</label>        \r\n    </div>\r\n    <div class=\"field input\">\r\n        <textarea id=\"TopicDescription\" data-i18n-placeholder=\"topic-description\">"
    + escapeExpression(((helper = (helper = helpers.TopicDescription || (depth0 != null ? depth0.TopicDescription : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TopicDescription","hash":{},"data":data}) : helper)))
    + "</textarea>\r\n        <label for=\"TopicDescription\" data-i18n=\"topic-description\">Description</label>\r\n    </div>\r\n    <button class=\"small ghost\" style=\"float:right;\" data-i18n=\"topic-delete\"></button>\r\n</form>";
},"useData":true});

this["fsTemplates"]["tError"] = Handlebars.template({"1":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "    <a class=\"button\" href=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.SteamPath : stack1), depth0))
    + "\"\r\n       data-i18n=\"error-steam-from\"\r\n       data-i18n-args=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.PlainName : stack1), depth0))
    + "\">Steam von"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.PlainName : stack1), depth0))
    + "</a>\r\n";
},"3":function(depth0,helpers,partials,data) {
  return "    <a class=\"button\" href=\"/start\" data-i18n=\"error-startpage\">Startseite</a>\r\n";
  },"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression, lambda=this.lambda, buffer = "<div class=\"box\">\r\n    <i class=\"err\">"
    + escapeExpression(((helper = (helper = helpers.ErrorNo || (depth0 != null ? depth0.ErrorNo : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"ErrorNo","hash":{},"data":data}) : helper)))
    + "</i>\r\n\r\n    <small style=\"font-size: 0.9em; color: #aaa;\">"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Route : depth0)) != null ? stack1.FullPath : stack1), depth0))
    + "</small>\r\n    <h1 style=\"margin-top: 0;\" data-i18n=\"error-title\">Arrrgh, da fehlt eine Seite!</h1>\r\n\r\n    <p data-i18n=\"error-decide\">Entscheide Du, wie es weitergehen soll:</p>\r\n\r\n";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.Member : depth0), {"name":"if","hash":{},"fn":this.program(1, data),"inverse":this.program(3, data),"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer + "\r\n    <p id=\"err-contact-info\" data-i18n=\"error-contact\"></p>\r\n\r\n    <div id=\"err-who-failed\">\r\n        <img id=\"err-kr\" data-dev=\"KRISTOF\" src=\"/Content/Images/rokr-kr-failed.png\" />\r\n        <img id=\"err-ro\" data-dev=\"ROBERT\" src=\"/Content/Images/rokr-ro-failed.png\" />\r\n    </div>\r\n    \r\n    <h2 data-i18n=\"contact\">Contact</h2>\r\n\r\n    <form id=\"err-contact-form\">\r\n        <div class=\"field input\">\r\n            <input id=\"ErrSubject\" name=\"ErrSubject\" type=\"text\"\r\n                   data-i18n-placeholder=\"contact-subject\"\r\n                   data-i18n-value=\"error-contact-default\"\r\n                   data-i18n-value-args=\""
    + escapeExpression(((helper = (helper = helpers.ErrorNo || (depth0 != null ? depth0.ErrorNo : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"ErrorNo","hash":{},"data":data}) : helper)))
    + "\" value=\""
    + escapeExpression(((helper = (helper = helpers.Subject || (depth0 != null ? depth0.Subject : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Subject","hash":{},"data":data}) : helper)))
    + "\" />\r\n            <label for=\"ErrSubject\" data-i18n=\"contact-subject\">Betreff</label>\r\n            <span id=\"errorErrSubject\" class='help-block'></span>\r\n        </div>\r\n        <div class=\"field input\">\r\n            <input id=\"ErrSenderName\" name=\"ErrSenderName\" type=\"text\"\r\n                   data-i18n-placeholder=\"your-name\" value=\""
    + escapeExpression(((helper = (helper = helpers.SenderName || (depth0 != null ? depth0.SenderName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"SenderName","hash":{},"data":data}) : helper)))
    + "\" />\r\n            <label for=\"ErrSenderName\" data-i18n=\"your-name\">SenderName</label>\r\n            <span id=\"errorErrSenderName\" class='help-block'></span>\r\n        </div>\r\n        <div class=\"field input\">\r\n            <input id=\"ErrSenderMail\" name=\"ErrSenderMail\" type=\"email\"\r\n                   data-i18n-placeholder=\"your-mail-address\" value=\""
    + escapeExpression(((helper = (helper = helpers.SenderMail || (depth0 != null ? depth0.SenderMail : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"SenderMail","hash":{},"data":data}) : helper)))
    + "\" />\r\n            <label for=\"ErrSenderMail\" data-i18n=\"your-mail-address\">EMail</label>\r\n            <span id=\"errorErrSenderMail\" class='help-block'></span>\r\n        </div>\r\n        <div class=\"field input\">\r\n            <textarea id=\"ErrMessage\" name=\"ErrMessage\" data-i18n-placeholder=\"contact-message\"></textarea>\r\n            <label for=\"ErrMessage\" data-i18n=\"contact-message\">Nachricht</label>\r\n        </div>\r\n        <input id=\"WhoFailed\" type=\"hidden\" value=\"NOBODY\" />\r\n        <a class=\"button small\" id=\"ErrContactSend\" data-i18n=\"error-contact-send\">Senden</a>\r\n    </form>\r\n\r\n</div>";
},"useData":true});

this["fsTemplates"]["tFigure"] = Handlebars.template({"1":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers['if'].call(depth0, ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.AllowCommenting : stack1), {"name":"if","hash":{},"fn":this.program(2, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"2":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "        <a class=\"figure-btn btn-comments icon icon-chat tooltip-left\" href=\"#\"\r\n           data-i18n-title=\"comments\">\r\n            <span>"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.CommentCount : stack1), depth0))
    + "</span>\r\n        </a>\r\n";
},"4":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers['if'].call(depth0, ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.AllowRating : stack1), {"name":"if","hash":{},"fn":this.program(5, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"5":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "        <a class=\"figure-btn btn-rating btn-popout icon icon-star tooltip-left\"\r\n           data-popout=\"rating-popout-"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.fId : stack1), depth0))
    + "\"\r\n           data-i18n-title=\"rating\">\r\n            <span>"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.RatingSum : stack1), depth0))
    + "</span>\r\n        </a>\r\n";
},"7":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers['if'].call(depth0, ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.AllowSharing : stack1), {"name":"if","hash":{},"fn":this.program(8, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"8":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "        <a class=\"figure-btn btn-share btn-popout icon icon-share tooltip-left\"\r\n           data-popout=\"sharing-popout-"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.fId : stack1), depth0))
    + "\"\r\n           data-i18n-title=\"share\"></a>\r\n";
},"10":function(depth0,helpers,partials,data) {
  return "            <i class=\"icon icon-lock\" i18n-title=\"private\"></i>\r\n";
  },"12":function(depth0,helpers,partials,data) {
  return "            <i class=\"icon icon-feather\" i18n-title=\"story-only\"></i>\r\n";
  },"14":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers['if'].call(depth0, ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.AllowRating : stack1), {"name":"if","hash":{},"fn":this.program(15, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"15":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = this.invokePartial(partials.FotoRating, '    ', 'FotoRating', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"17":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers['if'].call(depth0, ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.AllowSharing : stack1), {"name":"if","hash":{},"fn":this.program(18, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"18":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = this.invokePartial(partials.FotoSharing, '    ', 'FotoSharing', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression, buffer = "<figure id=\"figure-"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.fId : stack1), depth0))
    + "\"\r\n        data-share-image-url=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Url : stack1), depth0))
    + "\"\r\n        data-title=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Title : stack1), depth0))
    + "\">\r\n\r\n    <div class=\"figure-bar\">\r\n        <a class=\"figure-btn btn-show icon icon-fullscreen tooltip-left\"\r\n           href=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Url1920 : stack1), depth0))
    + "\"\r\n           data-i18n-title=\"full-screen\"\r\n           detailurl=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Url : stack1), depth0))
    + "\"\r\n           plainname=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.PlainName : stack1), depth0))
    + "\"\r\n           avatarurl=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Avatar100Url : stack1), depth0))
    + "\"\r\n           alias=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Alias : stack1), depth0))
    + "\"></a>\r\n        <a class=\"figure-btn btn-info icon icon-picture tooltip-left\"\r\n           data-i18n-title=\"photo-info\"></a>\r\n        <a class=\"figure-btn btn-exif icon icon-camera tooltip-left\"\r\n           data-i18n-title=\"exif-data\"></a>\r\n        \r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Options : stack1)) != null ? stack1.AllowComments : stack1), {"name":"if","hash":{},"fn":this.program(1, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Options : stack1)) != null ? stack1.AllowRating : stack1), {"name":"if","hash":{},"fn":this.program(4, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Options : stack1)) != null ? stack1.AllowSharing : stack1), {"name":"if","hash":{},"fn":this.program(7, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "    </div>\r\n\r\n    <div class=\"foto "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Orientation : stack1), depth0))
    + "\" id=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.fId : stack1), depth0))
    + "\" role=\"img\" aria-label=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Caption : stack1), depth0))
    + "\"\r\n         onclick=\"window.location.href = '"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Url : stack1), depth0))
    + "';\" style=\"cursor: pointer;\"></div>\r\n    <figcaption>\r\n        <div class=\"info-icons\">\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.IsPrivate : stack1), {"name":"if","hash":{},"fn":this.program(10, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.IsForStoryOnly : stack1), {"name":"if","hash":{},"fn":this.program(12, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "        </div>\r\n        <span>"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Caption : stack1), depth0))
    + "</span>\r\n    </figcaption>\r\n\r\n    <div class=\"figure-details\"></div>\r\n\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Options : stack1)) != null ? stack1.AllowRating : stack1), {"name":"if","hash":{},"fn":this.program(14, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Options : stack1)) != null ? stack1.AllowSharing : stack1), {"name":"if","hash":{},"fn":this.program(17, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n</figure>\r\n<style type=\"text/css\">\r\n    @media only screen and (max-width: 40.063em) {#"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.fId : stack1), depth0))
    + " {background-image: url(";
  stack1 = lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Url640 : stack1), depth0);
  if (stack1 != null) { buffer += stack1; }
  buffer += ");}}\r\n    @media only screen and (min-width: 40.063em) and (max-width: 64em) {#"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.fId : stack1), depth0))
    + " {background-image: url(";
  stack1 = lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Url1024 : stack1), depth0);
  if (stack1 != null) { buffer += stack1; }
  buffer += ");}}\r\n    @media only screen and (min-width: 64.063em) and (max-width: 90em) {#"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.fId : stack1), depth0))
    + " {background-image: url(";
  stack1 = lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Url1440 : stack1), depth0);
  if (stack1 != null) { buffer += stack1; }
  buffer += ");}}\r\n    @media only screen and (min-width: 90.063em) {#"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.fId : stack1), depth0))
    + " {background-image: url(";
  stack1 = lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Url1920 : stack1), depth0);
  if (stack1 != null) { buffer += stack1; }
  return buffer + ");}}\r\n</style>";
},"usePartial":true,"useData":true});

this["fsTemplates"]["tFigureDetails"] = Handlebars.template({"1":function(depth0,helpers,partials,data) {
  var stack1, helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression, lambda=this.lambda;
  return "        <h3 data-i18n=\"download-original\">Download der Originaldatei</h3>\r\n        <a class=\"icon icon-download download\"\r\n           href=\""
    + escapeExpression(((helper = (helper = helpers.UrlDownload || (depth0 != null ? depth0.UrlDownload : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"UrlDownload","hash":{},"data":data}) : helper)))
    + "\"\r\n           download=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Name : stack1), depth0))
    + "\">"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Name : stack1), depth0))
    + ".jpg&nbsp;&nbsp;&nbsp;"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Width : stack1), depth0))
    + "x"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Height : stack1), depth0))
    + "px</a>\r\n";
},"3":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, blockHelperMissing=helpers.blockHelperMissing, buffer = "        <h3 data-i18n=\"categories\">Kategorien</h3>\r\n";
  stack1 = blockHelperMissing.call(depth0, lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.CategoryList : stack1), depth0), {"name":"Foto.CategoryList","hash":{},"fn":this.program(4, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"4":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "        <a class=\"collection-item collection-category\"\r\n           data-type=\""
    + escapeExpression(((helper = (helper = helpers.CategoryType || (depth0 != null ? depth0.CategoryType : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"CategoryType","hash":{},"data":data}) : helper)))
    + "\"\r\n           href=\""
    + escapeExpression(((helper = (helper = helpers.CategoryUrl || (depth0 != null ? depth0.CategoryUrl : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"CategoryUrl","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.CategoryName || (depth0 != null ? depth0.CategoryName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"CategoryName","hash":{},"data":data}) : helper)))
    + "<i>"
    + escapeExpression(((helper = (helper = helpers.CategoryPhotoCount || (depth0 != null ? depth0.CategoryPhotoCount : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"CategoryPhotoCount","hash":{},"data":data}) : helper)))
    + "</i></a>\r\n";
},"6":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, blockHelperMissing=helpers.blockHelperMissing, buffer = "        <h3 data-i18n=\"topics\">Themen</h3>\r\n";
  stack1 = blockHelperMissing.call(depth0, lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.TopicList : stack1), depth0), {"name":"Foto.TopicList","hash":{},"fn":this.program(7, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"7":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "        <a class=\"collection-item collection-topic\"\r\n           href=\""
    + escapeExpression(((helper = (helper = helpers.TopicUrl || (depth0 != null ? depth0.TopicUrl : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TopicUrl","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.TopicName || (depth0 != null ? depth0.TopicName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TopicName","hash":{},"data":data}) : helper)))
    + "<i>"
    + escapeExpression(((helper = (helper = helpers.TopicPhotoCount || (depth0 != null ? depth0.TopicPhotoCount : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TopicPhotoCount","hash":{},"data":data}) : helper)))
    + "</i></a>\r\n";
},"9":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "        <h3 data-i18n=\"location\">Ort</h3>\r\n        <a class=\"collection-item collection-location\"\r\n           data-iso-code=\""
    + escapeExpression(((helper = (helper = helpers.LocationCountryIsoCode || (depth0 != null ? depth0.LocationCountryIsoCode : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationCountryIsoCode","hash":{},"data":data}) : helper)))
    + "\"\r\n           data-latitude=\""
    + escapeExpression(((helper = (helper = helpers.LocationLatitude || (depth0 != null ? depth0.LocationLatitude : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationLatitude","hash":{},"data":data}) : helper)))
    + "\"\r\n           data-longitude=\""
    + escapeExpression(((helper = (helper = helpers.LocationLongitude || (depth0 != null ? depth0.LocationLongitude : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationLongitude","hash":{},"data":data}) : helper)))
    + "\"\r\n           href=\""
    + escapeExpression(((helper = (helper = helpers.LocationUrl || (depth0 != null ? depth0.LocationUrl : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationUrl","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.LocationName || (depth0 != null ? depth0.LocationName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationName","hash":{},"data":data}) : helper)))
    + "<i>"
    + escapeExpression(((helper = (helper = helpers.LocationPhotoCount || (depth0 != null ? depth0.LocationPhotoCount : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationPhotoCount","hash":{},"data":data}) : helper)))
    + "</i></a>\r\n";
},"11":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "        <h3 data-i18n=\"event\">Veranstaltung</h3>\r\n        <a class=\"collection-item collection-event\"\r\n           data-date=\""
    + escapeExpression(((helper = (helper = helpers.EventDate || (depth0 != null ? depth0.EventDate : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"EventDate","hash":{},"data":data}) : helper)))
    + "\"\r\n           data-description=\""
    + escapeExpression(((helper = (helper = helpers.EventDescription || (depth0 != null ? depth0.EventDescription : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"EventDescription","hash":{},"data":data}) : helper)))
    + "\"\r\n           href=\""
    + escapeExpression(((helper = (helper = helpers.EventUrl || (depth0 != null ? depth0.EventUrl : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"EventUrl","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.EventName || (depth0 != null ? depth0.EventName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"EventName","hash":{},"data":data}) : helper)))
    + "<i>"
    + escapeExpression(((helper = (helper = helpers.EventPhotoCount || (depth0 != null ? depth0.EventPhotoCount : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"EventPhotoCount","hash":{},"data":data}) : helper)))
    + "</i></a>\r\n";
},"13":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, blockHelperMissing=helpers.blockHelperMissing, buffer = "        <h3 data-i18n=\"stories\">Geschichten</h3>\r\n";
  stack1 = blockHelperMissing.call(depth0, lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.StoryList : stack1), depth0), {"name":"Foto.StoryList","hash":{},"fn":this.program(14, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"14":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "        <a class=\"collection-item collection-story\"\r\n           href=\""
    + escapeExpression(((helper = (helper = helpers.StoryUrl || (depth0 != null ? depth0.StoryUrl : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"StoryUrl","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.StoryName || (depth0 != null ? depth0.StoryName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"StoryName","hash":{},"data":data}) : helper)))
    + "<i>"
    + escapeExpression(((helper = (helper = helpers.StoryPhotoCount || (depth0 != null ? depth0.StoryPhotoCount : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"StoryPhotoCount","hash":{},"data":data}) : helper)))
    + "</i></a>\r\n";
},"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression, blockHelperMissing=helpers.blockHelperMissing, buffer = "<div class=\"details-panel foto-meta panel-scroll\">    \r\n    <a class=\"foto-meta-pic "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Orientation : stack1), depth0))
    + "\" href=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Url : stack1), depth0))
    + "\">\r\n        <img src=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Url640 : stack1), depth0))
    + "\" />\r\n    </a>\r\n    <div class=\"foto-meta-sub\">\r\n\r\n        <h3 data-i18n=\"title\">Titel</h3>\r\n        <span class=\"info\">"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Caption : stack1), depth0))
    + "</span>\r\n\r\n        <h3 data-i18n=\"publication\">Veröffentlichung</h3>\r\n        <span class=\"info\">"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.PublishDate_Formatted : stack1), depth0))
    + "</span>\r\n\r\n        <h3 data-i18n=\"license\">Lizenz</h3>\r\n        <span class=\"info info-license\"><span></span>"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.License : stack1), depth0))
    + "<br /><em>"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.LicenseName : stack1), depth0))
    + "</em></span>\r\n\r\n";
  stack1 = blockHelperMissing.call(depth0, lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.AllowFullSizeDownload : stack1), depth0), {"name":"Foto.AllowFullSizeDownload","hash":{},"fn":this.program(1, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.CategoryList : stack1)) != null ? stack1.length : stack1), {"name":"if","hash":{},"fn":this.program(3, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.TopicList : stack1)) != null ? stack1.length : stack1), {"name":"if","hash":{},"fn":this.program(6, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = blockHelperMissing.call(depth0, lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Location : stack1), depth0), {"name":"Foto.Location","hash":{},"fn":this.program(9, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = blockHelperMissing.call(depth0, lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Event : stack1), depth0), {"name":"Foto.Event","hash":{},"fn":this.program(11, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.StoryList : stack1)) != null ? stack1.length : stack1), {"name":"if","hash":{},"fn":this.program(13, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n    </div>\r\n\r\n</div>\r\n\r\n<div class=\"details-panel panel-exif panel-scroll\">\r\n    <h3 data-i18n=\"exif-data\">EXIF-Daten der Kamera</h3>\r\n    <div class=\"flex-table\">\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"file-name\">Dateiname</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Name : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"capture-date\">Aufnahmedatum</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.CaptureDate : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"artist\">Autor</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.Artist : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"copyright\">Copyright</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.Copyright : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"description\">Beschreibung</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.Description : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"manufacturer\">Hersteller</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.Make : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"model\">Modell</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.Model : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"software\">Software</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.Software : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"focal-length\">Brennweite</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.FocalLength : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"aperture\">Blende</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.FNumber : stack1), depth0))
    + " ("
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.ApertureValue : stack1), depth0))
    + ")</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"exposure-time\">Belichtungszeit</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.ExposureTime : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"exposure-bias\">Belichtungskorrektur</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.ExposureBiasValue : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"exposure-program\">Belichtungsprogramm</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.ExposureProgram : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"exposure-mode\">Belichtungsmodus</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.ExposureMode : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"iso-speed-rating\">ISO-Lichtempfindlichkeit</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.ISOSpeedRatings : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"metering-mode\">Messmethode</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.MeteringMode : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"resolution\">Bildauflösung</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.XResolution : stack1), depth0))
    + " x "
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.YResolution : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"resolution-unit\">Auflösungseinheit</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.ResolutionUnit : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"orientation\">Bildformat</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.OrientationLocale : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"aspect-ratio\">Bildseitenverhältnis</strong>\r\n            <span>"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.AspectRatio : stack1), depth0))
    + "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"longitude\">GPS-Längengrad</strong>\r\n            <span>";
  stack1 = lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.LongitudeDeg : stack1), depth0);
  if (stack1 != null) { buffer += stack1; }
  buffer += "</span>\r\n        </div>\r\n        <div class=\"flex-cell\">\r\n            <strong data-i18n=\"latitude\">GPS-Breitengrad</strong>\r\n            <span>";
  stack1 = lambda(((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Exif : stack1)) != null ? stack1.LatitudeDeg : stack1), depth0);
  if (stack1 != null) { buffer += stack1; }
  return buffer + "</span>\r\n        </div>\r\n    </div>\r\n\r\n    <div class=\"exif-map\"></div>\r\n</div>\r\n\r\n<div class=\"details-panel foto-comments\"></div>\r\n";
},"useData":true});

this["fsTemplates"]["tFooter"] = Handlebars.template({"1":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "    <div id=\"moreinfo-background\">\r\n        <span data-i18n=\"background\">Hintergrundbild</span><br>\r\n        <a href=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Background : depth0)) != null ? stack1.OriginalUrl : stack1), depth0))
    + "\" target=\"_blank\"><strong>"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Background : depth0)) != null ? stack1.Title : stack1), depth0))
    + "</strong>, "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Background : depth0)) != null ? stack1.Artist : stack1), depth0))
    + ", "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Background : depth0)) != null ? stack1.License : stack1), depth0))
    + "</a>\r\n    </div>\r\n";
},"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, buffer = "<div id=\"appinfo\">\r\n    <div id=\"appinfo-logo\">\r\n        <a href=\"/\"><h2>Fotosteam</h2></a>\r\n        <p>\r\n            <span data-i18n=\"by\">by</span>&nbsp;<a href=\"http://www.rokr.io\" class=\"rokr\" target=\"_blank\">rokr.io</a><br />\r\n            <a href=\"/robert/about\">Robert Bakos</a> & <a href=\"/kristof/about\">Kristof Zerbe</a>\r\n        </p>\r\n    </div>\r\n    <div id=\"appinfo-social\" class=\"socialmedia\">\r\n        <a href=\"https://plus.google.com/+FotosteamPage\" class=\"gplus\" target=\"_blank\"></a>\r\n        <a href=\"https://www.facebook.com/pages/Fotosteam/1597537423790954\" class=\"facebook\" target=\"_blank\"></a>\r\n        <a href=\"https://twitter.com/fotosteam\" class=\"twitter\" target=\"_blank\"></a>\r\n        <a href=\"mailto:info@fotosteam.com\" class=\"mail\" target=\"_blank\"></a>\r\n    </div>    \r\n</div>\r\n<div id=\"moreinfo\">\r\n";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.Background : depth0), {"name":"if","hash":{},"fn":this.program(1, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer + "    <div id=\"moreinfo-app\"></div>\r\n    <div id=\"moreinfo-buttons\">\r\n        <a class=\"button small ghost\" data-i18n=\"legalinfo\" href=\"/legalinfo\">Impressum</a>\r\n    </div>\r\n</div>";
},"useData":true});

this["fsTemplates"]["tFoto"] = Handlebars.template({"1":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers['if'].call(depth0, ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.AllowRating : stack1), {"name":"if","hash":{},"fn":this.program(2, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"2":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "                    <a class=\"foto-btn btn-rating btn-popout icon icon-star tooltip-top\"\r\n                       data-popout=\"rating-popout-"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.fId : stack1), depth0))
    + "\"\r\n                       data-i18n-title=\"rating\">\r\n                        <span>"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.RatingSum : stack1), depth0))
    + "</span>\r\n                    </a>\r\n";
},"4":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers['if'].call(depth0, ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.AllowSharing : stack1), {"name":"if","hash":{},"fn":this.program(5, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"5":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "                    <a class=\"foto-btn btn-share btn-popout icon icon-share tooltip-top\"\r\n                       data-popout=\"sharing-popout-"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.fId : stack1), depth0))
    + "\"\r\n                       data-i18n-title=\"share\"></a>\r\n";
},"7":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, buffer = "                <h3 data-i18n=\"description\">Beschreibung</h3>\r\n                <span class=\"info\">";
  stack1 = lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.MDescription : stack1), depth0);
  if (stack1 != null) { buffer += stack1; }
  return buffer + "</span>\r\n";
},"9":function(depth0,helpers,partials,data) {
  return "                    <i class=\"icon icon-paper-plane\" style=\"float:right; opacity:0.1;\"></i>\r\n";
  },"11":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "                <h3 data-i18n=\"download-original\">Download der Originaldatei</h3>\r\n                <a class=\"icon icon-download download\"\r\n                   href=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.UrlDownload : stack1), depth0))
    + "\"\r\n                   download=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Name : stack1), depth0))
    + "\">"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Name : stack1), depth0))
    + ".jpg&nbsp;&nbsp;&nbsp;"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Width : stack1), depth0))
    + "x"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Height : stack1), depth0))
    + "px</a>\r\n";
},"13":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, blockHelperMissing=helpers.blockHelperMissing, buffer = "                <h3 data-i18n=\"categories\">Kategorien</h3>\r\n";
  stack1 = blockHelperMissing.call(depth0, lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.CategoryList : stack1), depth0), {"name":"Foto.CategoryList","hash":{},"fn":this.program(14, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"14":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                <a class=\"collection-item collection-category\"\r\n                   data-type=\""
    + escapeExpression(((helper = (helper = helpers.CategoryType || (depth0 != null ? depth0.CategoryType : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"CategoryType","hash":{},"data":data}) : helper)))
    + "\"\r\n                   href=\""
    + escapeExpression(((helper = (helper = helpers.CategoryUrl || (depth0 != null ? depth0.CategoryUrl : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"CategoryUrl","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.CategoryName || (depth0 != null ? depth0.CategoryName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"CategoryName","hash":{},"data":data}) : helper)))
    + "<i>"
    + escapeExpression(((helper = (helper = helpers.CategoryPhotoCount || (depth0 != null ? depth0.CategoryPhotoCount : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"CategoryPhotoCount","hash":{},"data":data}) : helper)))
    + "</i></a>\r\n";
},"16":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, blockHelperMissing=helpers.blockHelperMissing, buffer = "                <h3 data-i18n=\"topics\">Themen</h3>\r\n";
  stack1 = blockHelperMissing.call(depth0, lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.TopicList : stack1), depth0), {"name":"Foto.TopicList","hash":{},"fn":this.program(17, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"17":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                <a class=\"collection-item collection-topic\"\r\n                   href=\""
    + escapeExpression(((helper = (helper = helpers.TopicUrl || (depth0 != null ? depth0.TopicUrl : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TopicUrl","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.TopicName || (depth0 != null ? depth0.TopicName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TopicName","hash":{},"data":data}) : helper)))
    + "<i>"
    + escapeExpression(((helper = (helper = helpers.TopicPhotoCount || (depth0 != null ? depth0.TopicPhotoCount : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TopicPhotoCount","hash":{},"data":data}) : helper)))
    + "</i></a>\r\n";
},"19":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                <h3 data-i18n=\"location\">Ort</h3>\r\n                <a class=\"collection-item collection-location\"\r\n                   data-iso-code=\""
    + escapeExpression(((helper = (helper = helpers.LocationCountryIsoCode || (depth0 != null ? depth0.LocationCountryIsoCode : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationCountryIsoCode","hash":{},"data":data}) : helper)))
    + "\"\r\n                   data-latitude=\""
    + escapeExpression(((helper = (helper = helpers.LocationLatitude || (depth0 != null ? depth0.LocationLatitude : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationLatitude","hash":{},"data":data}) : helper)))
    + "\"\r\n                   data-longitude=\""
    + escapeExpression(((helper = (helper = helpers.LocationLongitude || (depth0 != null ? depth0.LocationLongitude : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationLongitude","hash":{},"data":data}) : helper)))
    + "\"\r\n                   href=\""
    + escapeExpression(((helper = (helper = helpers.LocationUrl || (depth0 != null ? depth0.LocationUrl : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationUrl","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.LocationName || (depth0 != null ? depth0.LocationName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationName","hash":{},"data":data}) : helper)))
    + "<i>"
    + escapeExpression(((helper = (helper = helpers.LocationPhotoCount || (depth0 != null ? depth0.LocationPhotoCount : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationPhotoCount","hash":{},"data":data}) : helper)))
    + "</i></a>\r\n";
},"21":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                <h3 data-i18n=\"event\">Veranstaltung</h3>\r\n                <a class=\"collection-item collection-event\"\r\n                   data-date=\""
    + escapeExpression(((helper = (helper = helpers.EventDate || (depth0 != null ? depth0.EventDate : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"EventDate","hash":{},"data":data}) : helper)))
    + "\"\r\n                   data-description=\""
    + escapeExpression(((helper = (helper = helpers.EventDescription || (depth0 != null ? depth0.EventDescription : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"EventDescription","hash":{},"data":data}) : helper)))
    + "\"\r\n                   href=\""
    + escapeExpression(((helper = (helper = helpers.EventUrl || (depth0 != null ? depth0.EventUrl : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"EventUrl","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.EventName || (depth0 != null ? depth0.EventName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"EventName","hash":{},"data":data}) : helper)))
    + "<i>"
    + escapeExpression(((helper = (helper = helpers.EventPhotoCount || (depth0 != null ? depth0.EventPhotoCount : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"EventPhotoCount","hash":{},"data":data}) : helper)))
    + "</i></a>\r\n";
},"23":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, blockHelperMissing=helpers.blockHelperMissing, buffer = "                <h3 data-i18n=\"stories\">Geschichten</h3>\r\n";
  stack1 = blockHelperMissing.call(depth0, lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.StoryList : stack1), depth0), {"name":"Foto.StoryList","hash":{},"fn":this.program(24, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"24":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                <a class=\"collection-item collection-story\"\r\n                   href=\""
    + escapeExpression(((helper = (helper = helpers.StoryUrl || (depth0 != null ? depth0.StoryUrl : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"StoryUrl","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.StoryName || (depth0 != null ? depth0.StoryName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"StoryName","hash":{},"data":data}) : helper)))
    + "<i>"
    + escapeExpression(((helper = (helper = helpers.StoryPhotoCount || (depth0 != null ? depth0.StoryPhotoCount : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"StoryPhotoCount","hash":{},"data":data}) : helper)))
    + "</i></a>\r\n";
},"26":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers['if'].call(depth0, ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.AllowRating : stack1), {"name":"if","hash":{},"fn":this.program(27, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"27":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = this.invokePartial(partials.FotoRating, '        ', 'FotoRating', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"29":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers['if'].call(depth0, ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.AllowSharing : stack1), {"name":"if","hash":{},"fn":this.program(30, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"30":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = this.invokePartial(partials.FotoSharing, '        ', 'FotoSharing', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression, blockHelperMissing=helpers.blockHelperMissing, buffer = "<div id=\"figure-"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.fId : stack1), depth0))
    + "\"\r\n        data-share-image-url=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Url : stack1), depth0))
    + "\"\r\n        data-title=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Title : stack1), depth0))
    + "\" style=\"padding-top: 0;\">\r\n        \r\n    <div class=\"foto-details page-details\">\r\n\r\n        <div class=\"details-panel foto-meta\">\r\n            <div class=\"foto-meta-pic "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Orientation : stack1), depth0))
    + "\">\r\n                <img src=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Url640 : stack1), depth0))
    + "\" data-id=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Id : stack1), depth0))
    + "\" />\r\n            </div>\r\n            <div class=\"foto-meta-sub\">\r\n\r\n                <div style=\"margin-bottom: 10px;\">\r\n                    <a class=\"foto-btn btn-show icon icon-fullscreen tooltip-top\" href=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Url1920 : stack1), depth0))
    + "\"\r\n                       data-i18n-title=\"full-screen\"></a>\r\n\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Options : stack1)) != null ? stack1.AllowRating : stack1), {"name":"if","hash":{},"fn":this.program(1, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Options : stack1)) != null ? stack1.AllowSharing : stack1), {"name":"if","hash":{},"fn":this.program(4, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "                </div>\r\n\r\n                <h3 data-i18n=\"title\">Titel</h3>\r\n                <span class=\"info\">"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Caption : stack1), depth0))
    + "</span>\r\n                \r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.MDescription : stack1), {"name":"if","hash":{},"fn":this.program(7, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n                <h3 data-i18n=\"publication\">Veröffentlichung</h3>\r\n                <span class=\"info\">\r\n                    "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.PublishDate_Formatted : stack1), depth0))
    + "\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.AllowPromoting : stack1), {"name":"if","hash":{},"fn":this.program(9, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "                </span>\r\n\r\n                <h3 data-i18n=\"license\">Lizenz</h3>\r\n                <span class=\"info info-license\"><span></span>"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.License : stack1), depth0))
    + "<br /><em>"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.LicenseName : stack1), depth0))
    + "</em></span>\r\n\r\n";
  stack1 = blockHelperMissing.call(depth0, lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.AllowFullSizeDownload : stack1), depth0), {"name":"Foto.AllowFullSizeDownload","hash":{},"fn":this.program(11, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.CategoryList : stack1)) != null ? stack1.length : stack1), {"name":"if","hash":{},"fn":this.program(13, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.TopicList : stack1)) != null ? stack1.length : stack1), {"name":"if","hash":{},"fn":this.program(16, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = blockHelperMissing.call(depth0, lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Location : stack1), depth0), {"name":"Foto.Location","hash":{},"fn":this.program(19, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = blockHelperMissing.call(depth0, lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Event : stack1), depth0), {"name":"Foto.Event","hash":{},"fn":this.program(21, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.StoryList : stack1)) != null ? stack1.length : stack1), {"name":"if","hash":{},"fn":this.program(23, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n            </div>\r\n        </div>\r\n\r\n        <div class=\"location-map\" style=\"display:none;\"></div>\r\n\r\n        <div class=\"details-panel foto-interaction\">\r\n            <div class=\"foto-comments\"></div>\r\n            <div class=\"foto-ratings\"></div>\r\n        </div>\r\n\r\n";
  stack1 = this.invokePartial(partials.FotoExif, '        ', 'FotoExif', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Options : stack1)) != null ? stack1.AllowRating : stack1), {"name":"if","hash":{},"fn":this.program(26, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Options : stack1)) != null ? stack1.AllowSharing : stack1), {"name":"if","hash":{},"fn":this.program(29, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer + "\r\n    </div>\r\n\r\n</div>";
},"usePartial":true,"useData":true});

this["fsTemplates"]["tFotoEdit"] = Handlebars.template({"1":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "                <a class=\"foto-btn btn-rating icon icon-star tooltip-top\"\r\n                   data-i18n-title=\"rating\">\r\n                    <span>"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.RatingSum : stack1), depth0))
    + "</span>\r\n                </a>\r\n";
},"3":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression;
  return "                <a class=\"foto-btn btn-share btn-popout icon icon-share tooltip-top\"\r\n                   data-popout=\"sharing-popout-"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.fId : stack1), depth0))
    + "\"\r\n                   data-i18n-title=\"share\"></a>\r\n";
},"5":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.Selected : depth0), {"name":"if","hash":{},"fn":this.program(6, data),"inverse":this.program(8, data),"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"6":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                    <option value=\""
    + escapeExpression(((helper = (helper = helpers.CategoryTypeValue || (depth0 != null ? depth0.CategoryTypeValue : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"CategoryTypeValue","hash":{},"data":data}) : helper)))
    + "\" selected=\"selected\">"
    + escapeExpression(((helper = (helper = helpers.CategoryName || (depth0 != null ? depth0.CategoryName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"CategoryName","hash":{},"data":data}) : helper)))
    + "</option>\r\n";
},"8":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                    <option value=\""
    + escapeExpression(((helper = (helper = helpers.CategoryTypeValue || (depth0 != null ? depth0.CategoryTypeValue : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"CategoryTypeValue","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.CategoryName || (depth0 != null ? depth0.CategoryName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"CategoryName","hash":{},"data":data}) : helper)))
    + "</option>\r\n";
},"10":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.Selected : depth0), {"name":"if","hash":{},"fn":this.program(11, data),"inverse":this.program(13, data),"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"11":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                    <option value=\""
    + escapeExpression(((helper = (helper = helpers.TopicId || (depth0 != null ? depth0.TopicId : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TopicId","hash":{},"data":data}) : helper)))
    + "\" selected=\"selected\">"
    + escapeExpression(((helper = (helper = helpers.TopicName || (depth0 != null ? depth0.TopicName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TopicName","hash":{},"data":data}) : helper)))
    + "</option>\r\n";
},"13":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                    <option value=\""
    + escapeExpression(((helper = (helper = helpers.TopicId || (depth0 != null ? depth0.TopicId : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TopicId","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.TopicName || (depth0 != null ? depth0.TopicName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TopicName","hash":{},"data":data}) : helper)))
    + "</option>\r\n";
},"15":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.Selected : depth0), {"name":"if","hash":{},"fn":this.program(16, data),"inverse":this.program(18, data),"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"16":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                    <option value=\""
    + escapeExpression(((helper = (helper = helpers.LocationId || (depth0 != null ? depth0.LocationId : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationId","hash":{},"data":data}) : helper)))
    + "\" selected=\"selected\">"
    + escapeExpression(((helper = (helper = helpers.LocationName || (depth0 != null ? depth0.LocationName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationName","hash":{},"data":data}) : helper)))
    + "</option>\r\n";
},"18":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                    <option value=\""
    + escapeExpression(((helper = (helper = helpers.LocationId || (depth0 != null ? depth0.LocationId : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationId","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.LocationName || (depth0 != null ? depth0.LocationName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationName","hash":{},"data":data}) : helper)))
    + "</option>\r\n";
},"20":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.Selected : depth0), {"name":"if","hash":{},"fn":this.program(21, data),"inverse":this.program(23, data),"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"21":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                    <option value=\""
    + escapeExpression(((helper = (helper = helpers.EventId || (depth0 != null ? depth0.EventId : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"EventId","hash":{},"data":data}) : helper)))
    + "\" selected=\"selected\">"
    + escapeExpression(((helper = (helper = helpers.EventName || (depth0 != null ? depth0.EventName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"EventName","hash":{},"data":data}) : helper)))
    + "</option>\r\n";
},"23":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                    <option value=\""
    + escapeExpression(((helper = (helper = helpers.EventId || (depth0 != null ? depth0.EventId : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"EventId","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.EventName || (depth0 != null ? depth0.EventName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"EventName","hash":{},"data":data}) : helper)))
    + "</option>\r\n";
},"25":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, blockHelperMissing=helpers.blockHelperMissing, buffer = "                <h3 data-i18n=\"stories\"></h3>\r\n";
  stack1 = blockHelperMissing.call(depth0, lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.StoryList : stack1), depth0), {"name":"Foto.StoryList","hash":{},"fn":this.program(26, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"26":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                <a class=\"collection-item collection-story\"\r\n                   href=\""
    + escapeExpression(((helper = (helper = helpers.StoryUrl || (depth0 != null ? depth0.StoryUrl : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"StoryUrl","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.StoryName || (depth0 != null ? depth0.StoryName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"StoryName","hash":{},"data":data}) : helper)))
    + "<i>"
    + escapeExpression(((helper = (helper = helpers.StoryPhotoCount || (depth0 != null ? depth0.StoryPhotoCount : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"StoryPhotoCount","hash":{},"data":data}) : helper)))
    + "</i></a>\r\n";
},"28":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.Selected : depth0), {"name":"if","hash":{},"fn":this.program(29, data),"inverse":this.program(31, data),"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"29":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                    <option value=\""
    + escapeExpression(((helper = (helper = helpers.Value || (depth0 != null ? depth0.Value : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Value","hash":{},"data":data}) : helper)))
    + "\" selected=\"selected\">"
    + escapeExpression(((helper = (helper = helpers.Caption || (depth0 != null ? depth0.Caption : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Caption","hash":{},"data":data}) : helper)))
    + "</option>\r\n";
},"31":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                    <option value=\""
    + escapeExpression(((helper = (helper = helpers.Value || (depth0 != null ? depth0.Value : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Value","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.Caption || (depth0 != null ? depth0.Caption : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Caption","hash":{},"data":data}) : helper)))
    + "</option>\r\n";
},"33":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = this.invokePartial(partials.FotoSharing, '        ', 'FotoSharing', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, helper, options, lambda=this.lambda, escapeExpression=this.escapeExpression, functionType="function", helperMissing=helpers.helperMissing, blockHelperMissing=helpers.blockHelperMissing, buffer = "<div id=\"figure-"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.fId : stack1), depth0))
    + "\"\r\n     data-share-image-url=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Url : stack1), depth0))
    + "\"\r\n     data-title=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Title : stack1), depth0))
    + "\" style=\"padding-top: 0;background-position:"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.PLeft : stack1), depth0))
    + "% "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.PTop : stack1), depth0))
    + "%;\">\r\n\r\n    <div class=\"foto-details page-details\">\r\n        <div class=\"details-panel foto-meta\">\r\n            <div class=\"foto-meta-pic "
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Orientation : stack1), depth0))
    + "\">\r\n\r\n                <img src=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Url640 : stack1), depth0))
    + "\" data-id=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Id : stack1), depth0))
    + "\" id=\"foto-meta-image\"\r\n                     data-top=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Top : stack1), depth0))
    + "\" data-left=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Left : stack1), depth0))
    + "\" />\r\n\r\n                <h3 data-i18n=\"foto-set-focalpoint\" style=\"margin-top: 20px;\"></h3>\r\n                <div id=\"focalpoint-selection\">\r\n                    <button id=\"activate-focalpoint-selection\" data-i18n=\"foto-set-focalpoint-activate\" class=\"small fotocolor\"></button>\r\n                    <button id=\"save-focalpoint-selection\" data-i18n=\"foto-set-focalpoint-save\" class=\"small fotocolor force\" style=\"display:none;\"></button>\r\n                    <button id=\"cancel-focalpoint-selection\" data-i18n=\"cancel\" class=\"small fotocolor\" style=\"display:none;\"></button>\r\n                    <label id=\"focalpoint-selection-info\" data-i18n=\"foto-set-focalpoint-info\"></label>\r\n                </div>\r\n\r\n                <h3 data-i18n=\"foto-color\" style=\"margin-top: 20px;\"></h3>\r\n                <div class=\"color-chooser\">\r\n                    <a id=\"color-reset\" href=\"#\" data-i18n-title=\"foto-reset-color\">\r\n                        <i class=\"icon icon-arrows-cw\"></i>\r\n                    </a>\r\n                    <label id=\"color-label\" for=\"Color\">"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.ColorHex : stack1), depth0))
    + "</label>\r\n                    <input id=\"Color\" type=\"color\" value=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.ColorHex : stack1), depth0))
    + "\" />\r\n                </div>\r\n\r\n            </div>\r\n            <div class=\"foto-meta-sub\">\r\n\r\n                <ul id=\"foto-menu\" class=\"menu\">\r\n                    <li>\r\n                        <a href=\"#\" class=\"icon icon-down-open\"></a>\r\n                        <ul>\r\n                            <li><a href=\"#\" data-i18n=\"foto-upload-new-version\" onclick=\"UploadNewFotoVersion("
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Id : stack1), depth0))
    + "); return false;\">&nbsp;<small data-i18n=\"foto-upload-new-version-info\"></small></a></li>\r\n                            <li><a href=\"#\" data-i18n=\"foto-delete\" onclick=\"deleteFoto("
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Id : stack1), depth0))
    + "); return false;\">&nbsp;<small data-i18n=\"foto-delete-info\"></small></a></li>\r\n                        </ul>\r\n                    </li>\r\n                </ul>\r\n                <form id=\"upload-new-version-form\" enctype=\"multipart/form-data\" style=\"display: none;\">\r\n                    <input type=\"file\" id=\"upload-new-version\" name=\"upload-new-version\" />\r\n                </form>\r\n\r\n                <a class=\"foto-btn btn-show icon icon-fullscreen tooltip-top\" href=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Url1920 : stack1), depth0))
    + "\"\r\n                   data-i18n-title=\"full-screen\"></a>\r\n\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Options : stack1)) != null ? stack1.AllowRating : stack1), {"name":"if","hash":{},"fn":this.program(1, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Options : stack1)) != null ? stack1.AllowSharing : stack1), {"name":"if","hash":{},"fn":this.program(3, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n                <h3 data-i18n=\"status\"></h3>\r\n                <input id=\"IsPrivate\" type=\"checkbox\" class=\"ios pubpriv\" "
    + escapeExpression(((helper = (helper = helpers.IsPrivateAttribute || (depth0 != null ? depth0.IsPrivateAttribute : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"IsPrivateAttribute","hash":{},"data":data}) : helper)))
    + " />\r\n\r\n                <h3 data-i18n=\"title\"></h3>\r\n                <input id=\"Title\" class=\"info edit\" type=\"text\" role=\"textbox\" value=\""
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Caption : stack1), depth0))
    + "\" />\r\n\r\n                <h3 data-i18n=\"description\"></h3>\r\n                <textarea rows=\"3\" id=\"Description\" class=\"mdd_editor info edit\" type=\"text\" role=\"textbox\">"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.Description : stack1), depth0))
    + "</textarea>\r\n\r\n                <h3 data-i18n=\"categories\"></h3>\r\n                <select id=\"Categories\" multiple data-placeholder=\""
    + escapeExpression(((helper = (helper = helpers.ChoosePlaceholder || (depth0 != null ? depth0.ChoosePlaceholder : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"ChoosePlaceholder","hash":{},"data":data}) : helper)))
    + "\">\r\n";
  stack1 = ((helper = (helper = helpers.Categories || (depth0 != null ? depth0.Categories : depth0)) != null ? helper : helperMissing),(options={"name":"Categories","hash":{},"fn":this.program(5, data),"inverse":this.noop,"data":data}),(typeof helper === functionType ? helper.call(depth0, options) : helper));
  if (!helpers.Categories) { stack1 = blockHelperMissing.call(depth0, stack1, options); }
  if (stack1 != null) { buffer += stack1; }
  buffer += "                </select>\r\n\r\n                <h3 data-i18n=\"topics\"></h3>\r\n                <select id=\"Topics\" multiple data-placeholder=\""
    + escapeExpression(((helper = (helper = helpers.ChoosePlaceholder || (depth0 != null ? depth0.ChoosePlaceholder : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"ChoosePlaceholder","hash":{},"data":data}) : helper)))
    + "\">\r\n";
  stack1 = ((helper = (helper = helpers.Topics || (depth0 != null ? depth0.Topics : depth0)) != null ? helper : helperMissing),(options={"name":"Topics","hash":{},"fn":this.program(10, data),"inverse":this.noop,"data":data}),(typeof helper === functionType ? helper.call(depth0, options) : helper));
  if (!helpers.Topics) { stack1 = blockHelperMissing.call(depth0, stack1, options); }
  if (stack1 != null) { buffer += stack1; }
  buffer += "                </select>\r\n\r\n                <h3 data-i18n=\"location\"></h3>\r\n                <select id=\"Location\" data-placeholder=\""
    + escapeExpression(((helper = (helper = helpers.ChoosePlaceholder || (depth0 != null ? depth0.ChoosePlaceholder : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"ChoosePlaceholder","hash":{},"data":data}) : helper)))
    + "\">\r\n                    <option value=\"0\" data-i18n=\"no-location\"></option>\r\n";
  stack1 = ((helper = (helper = helpers.Locations || (depth0 != null ? depth0.Locations : depth0)) != null ? helper : helperMissing),(options={"name":"Locations","hash":{},"fn":this.program(15, data),"inverse":this.noop,"data":data}),(typeof helper === functionType ? helper.call(depth0, options) : helper));
  if (!helpers.Locations) { stack1 = blockHelperMissing.call(depth0, stack1, options); }
  if (stack1 != null) { buffer += stack1; }
  buffer += "                </select>\r\n\r\n                <h3 data-i18n=\"event\"></h3>\r\n                <select id=\"Event\" data-placeholder=\""
    + escapeExpression(((helper = (helper = helpers.ChoosePlaceholder || (depth0 != null ? depth0.ChoosePlaceholder : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"ChoosePlaceholder","hash":{},"data":data}) : helper)))
    + "\">\r\n                    <option value=\"0\" data-i18n=\"no-event\"></option>\r\n";
  stack1 = ((helper = (helper = helpers.Events || (depth0 != null ? depth0.Events : depth0)) != null ? helper : helperMissing),(options={"name":"Events","hash":{},"fn":this.program(20, data),"inverse":this.noop,"data":data}),(typeof helper === functionType ? helper.call(depth0, options) : helper));
  if (!helpers.Events) { stack1 = blockHelperMissing.call(depth0, stack1, options); }
  if (stack1 != null) { buffer += stack1; }
  buffer += "                </select>\r\n\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.StoryList : stack1)) != null ? stack1.length : stack1), {"name":"if","hash":{},"fn":this.program(25, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n                <h3 data-i18n=\"license\"></h3>\r\n                <select id=\"License\">\r\n";
  stack1 = ((helper = (helper = helpers.Licenses || (depth0 != null ? depth0.Licenses : depth0)) != null ? helper : helperMissing),(options={"name":"Licenses","hash":{},"fn":this.program(28, data),"inverse":this.noop,"data":data}),(typeof helper === functionType ? helper.call(depth0, options) : helper));
  if (!helpers.Licenses) { stack1 = blockHelperMissing.call(depth0, stack1, options); }
  if (stack1 != null) { buffer += stack1; }
  buffer += "                </select>\r\n\r\n                <h3 data-i18n=\"download-original\"></h3>\r\n                <input id=\"AllowFullSizeDownload\" type=\"checkbox\" class=\"ios allowdeny\" "
    + escapeExpression(((helper = (helper = helpers.AllowFullSizeDownloadAttribute || (depth0 != null ? depth0.AllowFullSizeDownloadAttribute : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"AllowFullSizeDownloadAttribute","hash":{},"data":data}) : helper)))
    + " />\r\n\r\n                <h3 data-i18n=\"foto-commenting\"></h3>\r\n                <input id=\"AllowCommenting\" type=\"checkbox\" class=\"ios allowdeny\" "
    + escapeExpression(((helper = (helper = helpers.AllowCommentingAttribute || (depth0 != null ? depth0.AllowCommentingAttribute : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"AllowCommentingAttribute","hash":{},"data":data}) : helper)))
    + " />\r\n\r\n                <h3 data-i18n=\"foto-rating\"></h3>\r\n                <input id=\"AllowRating\" type=\"checkbox\" class=\"ios allowdeny\" "
    + escapeExpression(((helper = (helper = helpers.AllowRatingAttribute || (depth0 != null ? depth0.AllowRatingAttribute : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"AllowRatingAttribute","hash":{},"data":data}) : helper)))
    + " />\r\n\r\n                <h3 data-i18n=\"foto-sharing\"></h3>\r\n                <input id=\"AllowSharing\" type=\"checkbox\" class=\"ios allowdeny\" "
    + escapeExpression(((helper = (helper = helpers.AllowSharingAttribute || (depth0 != null ? depth0.AllowSharingAttribute : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"AllowSharingAttribute","hash":{},"data":data}) : helper)))
    + " />\r\n\r\n                <h3 data-i18n=\"show-in-overview\"></h3>\r\n                <input id=\"ShowInOverview\" type=\"checkbox\" class=\"ios yesno\" "
    + escapeExpression(((helper = (helper = helpers.ShowInOverviewAttribute || (depth0 != null ? depth0.ShowInOverviewAttribute : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"ShowInOverviewAttribute","hash":{},"data":data}) : helper)))
    + " />\r\n\r\n                <h3 data-i18n=\"story-only\"></h3>\r\n                <input id=\"IsForStoryOnly\" type=\"checkbox\" class=\"ios yesno\" "
    + escapeExpression(((helper = (helper = helpers.IsForStoryOnlyAttribute || (depth0 != null ? depth0.IsForStoryOnlyAttribute : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"IsForStoryOnlyAttribute","hash":{},"data":data}) : helper)))
    + " />\r\n\r\n                <h3 data-i18n=\"fotosteam-promote\"></h3>\r\n                <input id=\"AllowPromoting\" type=\"checkbox\" class=\"ios allowdeny\" "
    + escapeExpression(((helper = (helper = helpers.AllowPromotingAttribute || (depth0 != null ? depth0.AllowPromotingAttribute : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"AllowPromotingAttribute","hash":{},"data":data}) : helper)))
    + " />\r\n\r\n                <h3 data-i18n=\"publication\">Veröffentlichung</h3>\r\n                <span class=\"info\">"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Foto : depth0)) != null ? stack1.PublishDate_Formatted : stack1), depth0))
    + "</span>\r\n\r\n            </div>\r\n        </div>\r\n\r\n        <div class=\"location-map\" style=\"display: none;\"></div>\r\n\r\n        <div class=\"details-panel foto-interaction\">\r\n            <div class=\"foto-comments\"></div>\r\n            <div class=\"foto-ratings\"></div>\r\n        </div>\r\n\r\n";
  stack1 = this.invokePartial(partials.FotoExif, '        ', 'FotoExif', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Options : stack1)) != null ? stack1.AllowSharing : stack1), {"name":"if","hash":{},"fn":this.program(33, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer + "\r\n    </div>\r\n</div>\r\n";
},"usePartial":true,"useData":true});

this["fsTemplates"]["tLegalInfo"] = Handlebars.template({"1":function(depth0,helpers,partials,data) {
  return "    <p style=\"font-style: italic; margin-top: 20px;\" data-i18n=\"legalinfo-note\">\r\n        The German 'Telemediengesetz' (German Telemedia Act) requires that German\r\n        websites must have an 'Impressum' disclosing information about the publisher,\r\n        including their name and address, telephone number or e-mail address, and\r\n        other information. German websites are defined as being published by individuals\r\n        or organisations that are based in Germany, so an Impressum is required regardless\r\n        of whether a site is in the DE domain. There is no equivalent legislation in other\r\n        countries, so we provide it in <strong>german language only</strong>.\r\n    </p>\r\n";
  },"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, buffer = "<div class=\"box\">\r\n    <h1>Das Team</h1>\r\n    <div id=\"team\">\r\n        <div class=\"team-person\">\r\n            <img src=\"/Content/Images/portrait-robert-300q.jpg\">\r\n        </div>\r\n        <div class=\"team-person-info\">\r\n            <div class=\"team-person-left\">\r\n                <h2>Robert Bakos</h2>\r\n                <p>Backend-Magus</p>\r\n            </div>\r\n            <div class=\"team-person-right\">\r\n                <h2>Kristof Zerbe</h2>\r\n                <p>Frontend-Wizard</p>\r\n            </div>\r\n        </div>\r\n        <div class=\"team-person\">\r\n            <img src=\"/Content/Images/portrait-kristof-300q.jpg\">\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n<div class=\"box\">\r\n    <h1 style =\"margin-bottom: 0;\">Impressum</h1>\r\n";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.langDE : depth0), {"name":"if","hash":{},"fn":this.program(1, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer + "    <h4 class=\"strip\">Angaben gemäß § 5 TMG:</h4>\r\n    <p>\r\n        Die nachstehenden Informationen enthalten die gesetzlich vorgesehenen\r\n        Pflichtangaben zur Anbieterkennzeichnung auf dieser Website:\r\n    </p>\r\n    <p>\r\n        <strong>FOTOSTEAM</strong><br />\r\n        Kristof Zerbe<br />\r\n        Schöne Aussicht 2b<br />\r\n        65193 Wiesbaden<br />\r\n        <a href=\"mailto:kristof@fotosteam.com\">kristof@fotosteam.com</a>\r\n    </p>\r\n</div>\r\n\r\n<div class=\"box\">\r\n    <h1 style=\"margin-bottom: 0;\">Rechtliche Hinweise</h1>\r\n    <h4 class=\"strip\">Haftung für Inhalte</h4>\r\n    <p>\r\n        Als Diensteanbieter sind wir gemäß § 7 Abs.1 TMG für eigene Inhalte auf\r\n        dieser Website nach den allgemeinen Gesetzen verantwortlich.<br />\r\n        Nach §§ 8 bis 10 TMG sind wir als Diensteanbieter jedoch nicht verpflichtet,\r\n        übermittelte oder gespeicherte fremde Informationen zu überwachen oder nach\r\n        Umständen zu forschen, die auf eine rechtswidrige Tätigkeit hinweisen.\r\n        Verpflichtungen zur Entfernung oder Sperrung der Nutzung von Informationen\r\n        nach den allgemeinen Gesetzen bleiben hiervon unberührt.<br />\r\n        Eine diesbezügliche Haftung ist jedoch erst ab dem Zeitpunkt der Kenntnis\r\n        einer konkreten Rechtsverletzung möglich. Bei Bekanntwerden von entsprechenden\r\n        Rechtsverletzungen werden wir diese Inhalte umgehend entfernen.\r\n    </p>\r\n    <h4 class=\"strip\">Haftung für Links</h4>\r\n    <p>\r\n        Diese Website enthält Links zu externen Webseiten Dritter, auf deren Inhalte\r\n        wir keinen Einfluss haben. Deshalb können wir für diese fremden Inhalte auch\r\n        keine Gewähr übernehmen. Für die Inhalte der verlinkten Seiten ist stets der\r\n        jeweilige Anbieter oder Betreiber der Seiten verantwortlich. Die verlinkten\r\n        Seiten wurden zum Zeitpunkt der Verlinkung auf mögliche Rechtsverstöße überprüft.\r\n        Rechtswidrige Inhalte waren zum Zeitpunkt der Verlinkung nicht erkennbar.\r\n        Eine permanente inhaltliche Kontrolle der verlinkten Seiten ist jedoch ohne\r\n        konkrete Anhaltspunkte einer Rechtsverletzung nicht zumutbar. Bei Bekanntwerden\r\n        von Rechtsverletzungen werden wir derartige Links umgehend entfernen.\r\n    </p>\r\n    <h4 class=\"strip\">Urheberrecht</h4>\r\n    <p>\r\n        Die durch die Betreiber und die Mitglieder dieser Website erstellten Inhalte\r\n        und Werke unterliegen dem deutschen Urheberrecht.<br />\r\n        Die Vervielfältigung, Bearbeitung, Verbreitung und jede Art der Verwertung\r\n        außerhalb der Grenzen des Urheberrechtes bedürfen der schriftlichen Zustimmung\r\n        des jeweiligen Autors bzw. Erstellers. Downloads und Kopien von dieser Website\r\n        sind nur für den privaten, nicht kommerziellen Gebrauch gestattet. Soweit die\r\n        Inhalte auf dieser Website nicht vom Betreiber erstellt wurden, werden die\r\n        Urheberrechte Dritter beachtet. Insbesondere werden Inhalte Dritter als solche\r\n        gekennzeichnet.<br />\r\n        Sollten Sie trotzdem auf eine Urheberrechtsverletzung aufmerksam werden, bitten\r\n        wir um einen entsprechenden Hinweis. Bei Bekanntwerden von Rechtsverletzungen\r\n        werden wir derartige Inhalte umgehend entfernen.\r\n    </p>\r\n    <h4 class=\"strip\">Datenschutz</h4>\r\n    <p>\r\n        Die Nutzung unserer Website ist in der Regel ohne Angabe personenbezogener Daten\r\n        möglich. Soweit auf unseren Seiten personenbezogene Daten (beispielsweise Name oder\r\n        eMail-Adresse) erhoben werden, erfolgt dies, soweit möglich, stets auf freiwilliger\r\n        Basis. Diese Daten werden ohne Ihre ausdrückliche Zustimmung nicht an Dritte\r\n        weitergegeben.<br />\r\n        Wir weisen darauf hin, dass die Datenübertragung im Internet Sicherheitslücken\r\n        aufweisen kann. Ein lückenloser Schutz der Daten vor dem Zugriff durch Dritte ist\r\n        nicht möglich.<br />\r\n        Der Nutzung von im Rahmen der Impressumspflicht veröffentlichten Kontaktdaten durch\r\n        Dritte zur Übersendung von nicht ausdrücklich angeforderter Werbung und\r\n        Informationsmaterialien wird hiermit ausdrücklich widersprochen. Die Betreiber der\r\n        Website behalten sich ausdrücklich rechtliche Schritte im Falle der unverlangten\r\n        Zusendung von Werbeinformationen, etwa durch Spam-Mails, vor.\r\n    </p>\r\n    <h4 class=\"strip\">Rechtswirksamkeit dieses Haftungsausschlusses</h4>\r\n    <p>\r\n        Dieser Haftungsausschluss ist als Teil der Website zu betrachten,\r\n        von dem aus auf diese Seite verwiesen wurde. Sofern Teile oder einzelne\r\n        Formulierungen dieses Textes der geltenden Rechtslage nicht, nicht mehr oder\r\n        nicht vollständig entsprechen sollten, bleiben die übrigen Teile des Dokumentes\r\n        in ihrem Inhalt und ihrer Gültigkeit davon unberührt.\r\n    </p>\r\n\r\n</div>";
},"useData":true});

this["fsTemplates"]["tLogoMenu"] = Handlebars.template({"1":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers['if'].call(depth0, ((stack1 = (depth0 != null ? depth0.AuthMember : depth0)) != null ? stack1.IsRegistered : stack1), {"name":"if","hash":{},"fn":this.program(2, data),"inverse":this.program(4, data),"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"2":function(depth0,helpers,partials,data) {
  return "    <a class=\"tile tile-dashboard tooltip-menu\" id=\"tile-13\" href=\"/dashboard\" data-i18n-title=\"dashboard\">\r\n    <i class=\"icon icon-sliders\"></i>\r\n</a>\r\n";
  },"4":function(depth0,helpers,partials,data) {
  return "    <a class=\"tile tile-dashboard tooltip-menu\" id=\"tile-13\" href=\"/dashboard\" data-i18n-title=\"register-info\">\r\n    <i class=\"icon icon icon-user-add\"></i>\r\n</a>\r\n";
  },"6":function(depth0,helpers,partials,data) {
  return "<div class=\"tile\" id=\"tile-13\"></div>\r\n";
  },"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, buffer = "<a class=\"tile tile-menu tooltip-menu\" id=\"tile-11\" onclick=\"toggleSidebar();\" data-i18n-title=\"menu\">\r\n    <span></span><span></span><span></span><span></span>\r\n</a>\r\n<div class=\"tile\" id=\"tile-12\"></div>\r\n";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.AuthMember : depth0), {"name":"if","hash":{},"fn":this.program(1, data),"inverse":this.program(6, data),"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer + "<div class=\"tile\" id=\"tile-21\"></div>\r\n<div class=\"tile\" id=\"tile-22\"></div>\r\n<div class=\"tile\" id=\"tile-23\"></div>\r\n<div class=\"tile\" id=\"tile-31\"></div>\r\n<div class=\"tile\" id=\"tile-32\"></div>\r\n<div class=\"tile\" id=\"tile-33\"></div>\r\n<a class=\"tile tile-moveup tooltip-menu\" id=\"tile-41\" onclick=\"scrollUp();\" data-i18n-title=\"scroll-up\">\r\n    <i class=\"icon icon-angle-double-up\" style=\"display:none;\"></i>\r\n</a>\r\n<div class=\"tile\" id=\"tile-42\"></div>\r\n<div class=\"tile\" id=\"tile-43\"></div>\r\n";
},"useData":true});

this["fsTemplates"]["tMultiEditDialogBar"] = Handlebars.template({"1":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                <option value=\""
    + escapeExpression(((helper = (helper = helpers.CategoryTypeValue || (depth0 != null ? depth0.CategoryTypeValue : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"CategoryTypeValue","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.CategoryName || (depth0 != null ? depth0.CategoryName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"CategoryName","hash":{},"data":data}) : helper)))
    + "</option>\r\n";
},"3":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                <option value=\""
    + escapeExpression(((helper = (helper = helpers.TopicId || (depth0 != null ? depth0.TopicId : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TopicId","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.TopicName || (depth0 != null ? depth0.TopicName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TopicName","hash":{},"data":data}) : helper)))
    + "</option>\r\n";
},"5":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                <option value=\""
    + escapeExpression(((helper = (helper = helpers.LocationId || (depth0 != null ? depth0.LocationId : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationId","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.LocationName || (depth0 != null ? depth0.LocationName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationName","hash":{},"data":data}) : helper)))
    + "</option>\r\n";
},"7":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                <option value=\""
    + escapeExpression(((helper = (helper = helpers.EventId || (depth0 != null ? depth0.EventId : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"EventId","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.EventName || (depth0 != null ? depth0.EventName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"EventName","hash":{},"data":data}) : helper)))
    + "</option>\r\n";
},"9":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.Selected : depth0), {"name":"if","hash":{},"fn":this.program(10, data),"inverse":this.program(12, data),"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"10":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                <option value=\""
    + escapeExpression(((helper = (helper = helpers.Value || (depth0 != null ? depth0.Value : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Value","hash":{},"data":data}) : helper)))
    + "\" selected=\"selected\">"
    + escapeExpression(((helper = (helper = helpers.Caption || (depth0 != null ? depth0.Caption : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Caption","hash":{},"data":data}) : helper)))
    + "</option>\r\n";
},"12":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                <option value=\""
    + escapeExpression(((helper = (helper = helpers.Value || (depth0 != null ? depth0.Value : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Value","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.Caption || (depth0 != null ? depth0.Caption : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Caption","hash":{},"data":data}) : helper)))
    + "</option>\r\n";
},"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, helper, options, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression, blockHelperMissing=helpers.blockHelperMissing, buffer = "<div style=\"overflow: hidden;\">\r\n    <h3 id=\"multiedit-selected\" style=\"float: left\"></h3>\r\n    <div style=\"float: right\">\r\n        <button id=\"multiedit-delete\" class=\"small shine ghost\" data-i18n=\"delete\"></button>\r\n        <button id=\"multiedit-edit\" class=\"small shine\" data-i18n=\"edit\"></button>\r\n        <button id=\"multiedit-save\" class=\"small shine\" style=\"display:none;\" data-i18n=\"save\"></button>\r\n        <button id=\"multiedit-cancel\" class=\"small\" data-i18n=\"cancel\"></button>\r\n        <a id=\"multiedit-close\" class=\"icon icon-cancel\" style=\"display:none;\"></a>\r\n    </div>\r\n</div>\r\n<div id=\"multieditor\" style=\"display:none; margin-top: 20px;\">\r\n    <div id=\"multieditor-selection\" class=\"collection-container\">\r\n        <div class=\"collection-wrapper\"></div>\r\n    </div>\r\n    <div id=\"multieditor-controls\">\r\n\r\n        <div id=\"multieditor-controls-wrapper\">\r\n\r\n            <h3><a href=\"#\" data-i18n=\"status\"></a></h3>\r\n            <input id=\"IsPrivate\" type=\"checkbox\" class=\"ios pubpriv\" "
    + escapeExpression(((helper = (helper = helpers.IsPrivateAttribute || (depth0 != null ? depth0.IsPrivateAttribute : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"IsPrivateAttribute","hash":{},"data":data}) : helper)))
    + " />\r\n\r\n            <h3><a href=\"#\" data-i18n=\"categories\"></a></h3>\r\n            <select id=\"Categories\" multiple data-placeholder=\""
    + escapeExpression(((helper = (helper = helpers.ChoosePlaceholder || (depth0 != null ? depth0.ChoosePlaceholder : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"ChoosePlaceholder","hash":{},"data":data}) : helper)))
    + "\">\r\n";
  stack1 = ((helper = (helper = helpers.Categories || (depth0 != null ? depth0.Categories : depth0)) != null ? helper : helperMissing),(options={"name":"Categories","hash":{},"fn":this.program(1, data),"inverse":this.noop,"data":data}),(typeof helper === functionType ? helper.call(depth0, options) : helper));
  if (!helpers.Categories) { stack1 = blockHelperMissing.call(depth0, stack1, options); }
  if (stack1 != null) { buffer += stack1; }
  buffer += "            </select>\r\n\r\n            <h3><a href=\"#\" data-i18n=\"topics\"></a></h3>\r\n            <select id=\"Topics\" multiple data-placeholder=\""
    + escapeExpression(((helper = (helper = helpers.ChoosePlaceholder || (depth0 != null ? depth0.ChoosePlaceholder : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"ChoosePlaceholder","hash":{},"data":data}) : helper)))
    + "\">\r\n";
  stack1 = ((helper = (helper = helpers.Topics || (depth0 != null ? depth0.Topics : depth0)) != null ? helper : helperMissing),(options={"name":"Topics","hash":{},"fn":this.program(3, data),"inverse":this.noop,"data":data}),(typeof helper === functionType ? helper.call(depth0, options) : helper));
  if (!helpers.Topics) { stack1 = blockHelperMissing.call(depth0, stack1, options); }
  if (stack1 != null) { buffer += stack1; }
  buffer += "            </select>\r\n\r\n            <h3><a href=\"#\" data-i18n=\"location\"></a></h3>\r\n            <select id=\"Location\" data-placeholder=\""
    + escapeExpression(((helper = (helper = helpers.ChoosePlaceholder || (depth0 != null ? depth0.ChoosePlaceholder : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"ChoosePlaceholder","hash":{},"data":data}) : helper)))
    + "\">\r\n                <option value=\"0\" data-i18n=\"no-location\"></option>\r\n";
  stack1 = ((helper = (helper = helpers.Locations || (depth0 != null ? depth0.Locations : depth0)) != null ? helper : helperMissing),(options={"name":"Locations","hash":{},"fn":this.program(5, data),"inverse":this.noop,"data":data}),(typeof helper === functionType ? helper.call(depth0, options) : helper));
  if (!helpers.Locations) { stack1 = blockHelperMissing.call(depth0, stack1, options); }
  if (stack1 != null) { buffer += stack1; }
  buffer += "            </select>\r\n\r\n            <h3><a href=\"#\" data-i18n=\"event\"></a></h3>\r\n            <select id=\"Event\" data-placeholder=\""
    + escapeExpression(((helper = (helper = helpers.ChoosePlaceholder || (depth0 != null ? depth0.ChoosePlaceholder : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"ChoosePlaceholder","hash":{},"data":data}) : helper)))
    + "\">\r\n                <option value=\"0\" data-i18n=\"no-event\"></option>\r\n";
  stack1 = ((helper = (helper = helpers.Events || (depth0 != null ? depth0.Events : depth0)) != null ? helper : helperMissing),(options={"name":"Events","hash":{},"fn":this.program(7, data),"inverse":this.noop,"data":data}),(typeof helper === functionType ? helper.call(depth0, options) : helper));
  if (!helpers.Events) { stack1 = blockHelperMissing.call(depth0, stack1, options); }
  if (stack1 != null) { buffer += stack1; }
  buffer += "            </select>\r\n\r\n            <h3><a href=\"#\" data-i18n=\"license\"></a></h3>\r\n            <select id=\"License\">\r\n";
  stack1 = ((helper = (helper = helpers.Licenses || (depth0 != null ? depth0.Licenses : depth0)) != null ? helper : helperMissing),(options={"name":"Licenses","hash":{},"fn":this.program(9, data),"inverse":this.noop,"data":data}),(typeof helper === functionType ? helper.call(depth0, options) : helper));
  if (!helpers.Licenses) { stack1 = blockHelperMissing.call(depth0, stack1, options); }
  if (stack1 != null) { buffer += stack1; }
  return buffer + "            </select>\r\n\r\n            <h3><a href=\"#\" data-i18n=\"download-original\"></a></h3>\r\n            <input id=\"AllowFullSizeDownload\" type=\"checkbox\" class=\"ios allowdeny\" "
    + escapeExpression(((helper = (helper = helpers.AllowFullSizeDownloadAttribute || (depth0 != null ? depth0.AllowFullSizeDownloadAttribute : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"AllowFullSizeDownloadAttribute","hash":{},"data":data}) : helper)))
    + " />\r\n\r\n            <h3><a href=\"#\" data-i18n=\"foto-commenting\"></a></h3>\r\n            <input id=\"AllowCommenting\" type=\"checkbox\" class=\"ios allowdeny\" "
    + escapeExpression(((helper = (helper = helpers.AllowCommentingAttribute || (depth0 != null ? depth0.AllowCommentingAttribute : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"AllowCommentingAttribute","hash":{},"data":data}) : helper)))
    + " />\r\n\r\n            <h3><a href=\"#\" data-i18n=\"foto-rating\"></a></h3>\r\n            <input id=\"AllowRating\" type=\"checkbox\" class=\"ios allowdeny\" "
    + escapeExpression(((helper = (helper = helpers.AllowRatingAttribute || (depth0 != null ? depth0.AllowRatingAttribute : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"AllowRatingAttribute","hash":{},"data":data}) : helper)))
    + " />\r\n\r\n            <h3><a href=\"#\" data-i18n=\"foto-sharing\"></a></h3>\r\n            <input id=\"AllowSharing\" type=\"checkbox\" class=\"ios allowdeny\" "
    + escapeExpression(((helper = (helper = helpers.AllowSharingAttribute || (depth0 != null ? depth0.AllowSharingAttribute : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"AllowSharingAttribute","hash":{},"data":data}) : helper)))
    + " />\r\n\r\n            <h3><a href=\"#\" data-i18n=\"story-only\"></a></h3>\r\n            <input id=\"IsForStoryOnly\" type=\"checkbox\" class=\"ios yesno\" "
    + escapeExpression(((helper = (helper = helpers.IsForStoryOnlyAttribute || (depth0 != null ? depth0.IsForStoryOnlyAttribute : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"IsForStoryOnlyAttribute","hash":{},"data":data}) : helper)))
    + " />\r\n\r\n            <h3><a href=\"#\" data-i18n=\"fotosteam-promote\"></a></h3>\r\n            <input id=\"AllowPromoting\" type=\"checkbox\" class=\"ios allowdeny\" "
    + escapeExpression(((helper = (helper = helpers.AllowPromotingAttribute || (depth0 != null ? depth0.AllowPromotingAttribute : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"AllowPromotingAttribute","hash":{},"data":data}) : helper)))
    + " />\r\n\r\n        </div>\r\n\r\n    </div>\r\n</div>\r\n\r\n";
},"useData":true});

this["fsTemplates"]["tNewEvent"] = Handlebars.template({"1":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.Selected : depth0), {"name":"if","hash":{},"fn":this.program(2, data),"inverse":this.program(4, data),"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"2":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "            <option value=\""
    + escapeExpression(((helper = (helper = helpers.LocationId || (depth0 != null ? depth0.LocationId : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationId","hash":{},"data":data}) : helper)))
    + "\" selected=\"selected\">"
    + escapeExpression(((helper = (helper = helpers.LocationName || (depth0 != null ? depth0.LocationName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationName","hash":{},"data":data}) : helper)))
    + "</option>\r\n";
},"4":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "            <option value=\""
    + escapeExpression(((helper = (helper = helpers.LocationId || (depth0 != null ? depth0.LocationId : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationId","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.LocationName || (depth0 != null ? depth0.LocationName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationName","hash":{},"data":data}) : helper)))
    + "</option>\r\n";
},"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, helper, options, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression, blockHelperMissing=helpers.blockHelperMissing, buffer = "<form id=\"event-new-form\">\r\n    <div class=\"field input\">\r\n        <input id=\"EventName\" name=\"EventName\" type=\"text\"\r\n               data-i18n-placeholder=\"event-name\" value=\"\" />\r\n        <label for=\"EventName\" data-i18n=\"event-name\">Name</label>\r\n        <span id=\"errorEventName\" class='help-block'></span>\r\n    </div>\r\n    <div class=\"field input\">\r\n        <input id=\"Date\" name=\"Date\" type=\"text\"\r\n               data-i18n-placeholder=\"event-date\" value=\"\" />\r\n        <label for=\"Date\" data-i18n=\"event-date\">Datum</label>\r\n        <span id=\"errorDate\" class='help-block'></span>        \r\n    </div>\r\n    <div class=\"field input\">\r\n        <input id=\"DateTo\" name=\"DateTo\" type=\"text\"\r\n               data-i18n-placeholder=\"event-date-to\" value=\"\" />\r\n        <label for=\"DateTo\" data-i18n=\"event-date-to\">Datum bis</label>\r\n        <span id=\"errorDateTo\" class='help-block'></span>\r\n    </div>\r\n    <div class=\"field\">\r\n        <label for=\"EventLocationId\" class=\"input-label\" data-i18n=\"location\"></label>\r\n        <select id=\"EventLocationId\"\r\n                data-i18n-placeholder=\"event-location\"\r\n                data-placeholder=\""
    + escapeExpression(((helper = (helper = helpers.ChoosePlaceholder || (depth0 != null ? depth0.ChoosePlaceholder : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"ChoosePlaceholder","hash":{},"data":data}) : helper)))
    + "\">\r\n            <option value=\"0\" data-i18n=\"no-location\"></option>\r\n";
  stack1 = ((helper = (helper = helpers.Locations || (depth0 != null ? depth0.Locations : depth0)) != null ? helper : helperMissing),(options={"name":"Locations","hash":{},"fn":this.program(1, data),"inverse":this.noop,"data":data}),(typeof helper === functionType ? helper.call(depth0, options) : helper));
  if (!helpers.Locations) { stack1 = blockHelperMissing.call(depth0, stack1, options); }
  if (stack1 != null) { buffer += stack1; }
  return buffer + "        </select>\r\n    </div>\r\n    <div class=\"field input\">\r\n        <textarea id=\"EventDescription\" data-i18n-placeholder=\"event-description\"></textarea>\r\n        <label for=\"EventDescription\" data-i18n=\"event-description\">Description</label>\r\n    </div>\r\n    <button class=\"small\" data-i18n=\"event-new-save\"></button>\r\n    <input id=\"EventDate\" name=\"EventDate\" type=\"hidden\" />\r\n    <input id=\"EventDateTo\" name=\"EventDateTo\" type=\"hidden\" />\r\n</form>";
},"useData":true});

this["fsTemplates"]["tNewLocation"] = Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  return "<form id=\"location-new-form\">\r\n    <div id=\"location-map\"></div>\r\n    <div class=\"field input\">\r\n        <input id=\"LocationName\" name=\"LocationName\" type=\"text\"\r\n               data-i18n-placeholder=\"location-name\" value=\"\" />\r\n        <label for=\"LocationName\" data-i18n=\"location-name\">Name</label>\r\n        <span id=\"errorLocationName\" class='help-block'></span>\r\n    </div>\r\n    <div class=\"field input\">\r\n        <input id=\"LocationLatitude\" name=\"LocationLatitude\" type=\"text\"\r\n               data-i18n-placeholder=\"location-latitude\" value=\"\" />\r\n        <label for=\"LocationLatitude\" data-i18n=\"location-latitude\">Latitude</label>\r\n    </div>\r\n    <div class=\"field input\">\r\n        <input id=\"LocationLongitude\" name=\"LocationLongitude\" type=\"text\"\r\n               data-i18n-placeholder=\"location-longitude\" value=\"\" />\r\n        <label for=\"LocationLongitude\" data-i18n=\"location-longitude\">Longitude</label>\r\n    </div>\r\n    <div class=\"field input\">\r\n        <input id=\"LocationStreet\" name=\"LocationStreet\" type=\"text\"\r\n               data-i18n-placeholder=\"location-street\" value=\"\" />\r\n        <label for=\"LocationStreet\" data-i18n=\"location-street\">Street</label>\r\n    </div>\r\n    <div class=\"field input\">\r\n        <input id=\"LocationCity\" name=\"LocationCity\" type=\"text\"\r\n               data-i18n-placeholder=\"location-city\" value=\"\" />\r\n        <label for=\"LocationCity\" data-i18n=\"location-city\">City</label>\r\n        <!--<span id=\"errorLocationCity\" class='help-block'></span>-->\r\n    </div>\r\n    <div class=\"field input\">\r\n        <input id=\"LocationCounty\" name=\"LocationCounty\" type=\"text\"\r\n               data-i18n-placeholder=\"location-county\" value=\"\" />\r\n        <label for=\"LocationCounty\" data-i18n=\"location-county\">County</label>\r\n        <!--<span id=\"errorLocationCounty\" class='help-block'></span>-->\r\n    </div>\r\n    <div class=\"field input\">\r\n        <input id=\"LocationCountry\" name=\"LocationCountry\" type=\"text\"\r\n               data-i18n-placeholder=\"location-country\" value=\"\" />\r\n        <label for=\"LocationCountry\" data-i18n=\"location-country\">Country</label>\r\n        <span id=\"errorLocationCountry\" class='help-block'></span>\r\n    </div>\r\n    <div class=\"field input\">\r\n        <input id=\"LocationCountryIsoCode\" name=\"LocationCountryIsoCode\" type=\"text\"\r\n               data-i18n-placeholder=\"location-country-iso-code\" value=\"\" />\r\n        <label for=\"LocationCountryIsoCode\" data-i18n=\"location-country-iso-code\">CountryIsoCode</label>\r\n        <span id=\"errorLocationCountryIsoCode\" class='help-block'></span>\r\n    </div>\r\n    <button class=\"small\" data-i18n=\"location-new-save\"></button>\r\n</form>";
  },"useData":true});

this["fsTemplates"]["tNewTopic"] = Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  return "<form id=\"topic-new-form\">\r\n    <div class=\"field input\">\r\n        <input id=\"TopicName\" name=\"TopicName\" type=\"text\"\r\n               data-i18n-placeholder=\"topic-name\" value=\"\" />\r\n        <label for=\"TopicName\" data-i18n=\"topic-name\">Name</label>\r\n        <span id=\"errorTopicName\" class='help-block'></span>\r\n    </div>\r\n    <div class=\"field input\">\r\n        <textarea id=\"TopicDescription\" data-i18n-placeholder=\"topic-description\"></textarea>\r\n        <label for=\"TopicDescription\" data-i18n=\"topic-description\">Description</label>\r\n    </div>\r\n    <button class=\"small\" data-i18n=\"topic-new-save\"></button>\r\n</form>";
  },"useData":true});

this["fsTemplates"]["tNotificationUnread"] = Handlebars.template({"1":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "        <img src=\""
    + escapeExpression(((helper = (helper = helpers.TypeImageUrl || (depth0 != null ? depth0.TypeImageUrl : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TypeImageUrl","hash":{},"data":data}) : helper)))
    + "\" />\r\n";
},"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression, buffer = "<li>\r\n    <a class=\"dismiss icon icon-cancel-thin\"></a>\r\n    <a class=\"go\" data-id=\""
    + escapeExpression(((helper = (helper = helpers.Id || (depth0 != null ? depth0.Id : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Id","hash":{},"data":data}) : helper)))
    + "\" data-url=\""
    + escapeExpression(((helper = (helper = helpers.TypeUrl || (depth0 != null ? depth0.TypeUrl : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TypeUrl","hash":{},"data":data}) : helper)))
    + "\" href=\"#\">\r\n";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.TypeImageUrl : depth0), {"name":"if","hash":{},"fn":this.program(1, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer + "        <div>\r\n            <small>"
    + escapeExpression(((helper = (helper = helpers.Date || (depth0 != null ? depth0.Date : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Date","hash":{},"data":data}) : helper)))
    + "</small>\r\n            <span>"
    + escapeExpression(((helper = (helper = helpers.TypeCaption || (depth0 != null ? depth0.TypeCaption : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TypeCaption","hash":{},"data":data}) : helper)))
    + "</span>\r\n        </div>\r\n    </a>\r\n</li>";
},"useData":true});

this["fsTemplates"]["tNotificationUnreadGroup"] = Handlebars.template({"1":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "        <img src=\""
    + escapeExpression(((helper = (helper = helpers.PhotoUrl || (depth0 != null ? depth0.PhotoUrl : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"PhotoUrl","hash":{},"data":data}) : helper)))
    + "\" />\r\n";
},"3":function(depth0,helpers,partials,data) {
  var stack1, helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression, buffer = "                <div class=\"item\">\r\n";
  stack1 = helpers.unless.call(depth0, (depth0 != null ? depth0.PhotoUrl : depth0), {"name":"unless","hash":{},"fn":this.program(4, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer + "                    <a class=\"go\" data-id=\""
    + escapeExpression(((helper = (helper = helpers.Id || (depth0 != null ? depth0.Id : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Id","hash":{},"data":data}) : helper)))
    + "\" data-url=\""
    + escapeExpression(((helper = (helper = helpers.TypeUrl || (depth0 != null ? depth0.TypeUrl : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TypeUrl","hash":{},"data":data}) : helper)))
    + "\" href=\"#\">\r\n                        <small>"
    + escapeExpression(((helper = (helper = helpers.Date || (depth0 != null ? depth0.Date : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Date","hash":{},"data":data}) : helper)))
    + "</small>\r\n                        <span>"
    + escapeExpression(((helper = (helper = helpers.TypeCaption || (depth0 != null ? depth0.TypeCaption : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TypeCaption","hash":{},"data":data}) : helper)))
    + "</span>\r\n                    </a>\r\n                </div>\r\n";
},"4":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.TypeImageUrl : depth0), {"name":"if","hash":{},"fn":this.program(5, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"5":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                    <img src=\""
    + escapeExpression(((helper = (helper = helpers.TypeImageUrl || (depth0 != null ? depth0.TypeImageUrl : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TypeImageUrl","hash":{},"data":data}) : helper)))
    + "\" />\r\n";
},"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, helper, options, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression, blockHelperMissing=helpers.blockHelperMissing, buffer = "<li class=\"group\">\r\n    <a class=\"dismiss icon icon-cancel-thin\"></a>\r\n    <div href=\"#\" class=\"toggle\">\r\n";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.PhotoUrl : depth0), {"name":"if","hash":{},"fn":this.program(1, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "        <div>\r\n            <span>"
    + escapeExpression(((helper = (helper = helpers.TypeCaption || (depth0 != null ? depth0.TypeCaption : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TypeCaption","hash":{},"data":data}) : helper)))
    + "</span>\r\n            <div class=\"items\">\r\n";
  stack1 = ((helper = (helper = helpers.Notifications || (depth0 != null ? depth0.Notifications : depth0)) != null ? helper : helperMissing),(options={"name":"Notifications","hash":{},"fn":this.program(3, data),"inverse":this.noop,"data":data}),(typeof helper === functionType ? helper.call(depth0, options) : helper));
  if (!helpers.Notifications) { stack1 = blockHelperMissing.call(depth0, stack1, options); }
  if (stack1 != null) { buffer += stack1; }
  return buffer + "            </div>\r\n        </div>\r\n    </div>\r\n</li>";
},"useData":true});

this["fsTemplates"]["tNotifications"] = Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  return "<li style=\"background-color: transparent; padding: 0;\">\r\n    <p data-i18n=\"notification-no-unread\" style=\"margin-top: 0;\"></p>\r\n</li>";
  },"useData":true});

this["fsTemplates"]["tOverviewGrid"] = Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  return "<div id=\"overview\">\r\n    <div class=\"page-commands\">\r\n        <a id=\"overview-scale-down\" href=\"#\" class=\"tooltip-bottom\" data-i18n-title=\"key-scale-down\" style=\"padding-right: 0;\"><i class=\"icon icon-grid\"></i></a>\r\n        <div id=\"overview-scale\">\r\n            <a id=\"overview-scale-auto\">auto</a>\r\n            <div class=\"scale-block scale-10\"></div>\r\n            <div class=\"scale-block scale-9\"></div>\r\n            <div class=\"scale-block scale-8\"></div>\r\n            <div class=\"scale-block scale-7\"></div>\r\n            <div class=\"scale-block scale-6\"></div>\r\n            <div class=\"scale-block scale-5\"></div>\r\n            <div class=\"scale-block scale-4\"></div>\r\n            <div class=\"scale-block scale-3\"></div>\r\n        </div>\r\n        <a id=\"overview-scale-up\" href=\"#\" class=\"tooltip-bottom\" data-i18n-title=\"key-scale-up\"><i class=\"icon icon-grid-large\"></i></a>\r\n        <a id=\"overview-go-list\" href=\"#\" class=\"tooltip-bottom\" data-i18n-title=\"list-view\"><i class=\"icon icon-list\"></i></a>\r\n    </div>\r\n    <div class=\"collection-container\">\r\n        <div class=\"collection-wrapper\"></div>        \r\n    </div>\r\n</div>";
  },"useData":true});

this["fsTemplates"]["tOverviewList"] = Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "<div id=\"overview-list\">\r\n    <div class=\"page-commands\">\r\n        <a id=\"overview-go-grid\" href=\"#\" class=\"tooltip-bottom\" data-i18n-title=\"grid-view\"><i class=\"icon icon-grid-large\"></i></a>        \r\n    </div>\r\n    <ul>\r\n        <li class=\"list-header\">\r\n            <strong>"
    + escapeExpression(((helper = (helper = helpers.FotoCount || (depth0 != null ? depth0.FotoCount : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"FotoCount","hash":{},"data":data}) : helper)))
    + "</strong>\r\n            <a href=\"#\" class=\"item-id\">\r\n                <span>Id</span><br />\r\n                <span data-i18n=\"orientation\"></span>\r\n            </a>\r\n            <a href=\"#\" class=\"item-name\">\r\n                <span data-i18n=\"title\"></span><br />\r\n                <span data-i18n=\"file-name\"></span>\r\n            </a>\r\n            <a href=\"#\" class=\"item-date\">\r\n                <span data-i18n=\"capture-date\"></span><br />\r\n                <span data-i18n=\"publish-date\"></span>\r\n            </a>\r\n        </li>\r\n    </ul>\r\n</div>\r\n";
},"useData":true});

this["fsTemplates"]["tOverviewListItem"] = Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "<li class=\"list-item\">\r\n    <a href=\""
    + escapeExpression(((helper = (helper = helpers.Url || (depth0 != null ? depth0.Url : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Url","hash":{},"data":data}) : helper)))
    + "\" class=\"foto-btn btn-show icon icon-fullscreen tooltip-top\">\r\n        <img src=\""
    + escapeExpression(((helper = (helper = helpers.Url200 || (depth0 != null ? depth0.Url200 : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Url200","hash":{},"data":data}) : helper)))
    + "\" />\r\n        <span class=\"item-id\">"
    + escapeExpression(((helper = (helper = helpers.Id || (depth0 != null ? depth0.Id : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Id","hash":{},"data":data}) : helper)))
    + "<br />"
    + escapeExpression(((helper = (helper = helpers.OrientationLocale || (depth0 != null ? depth0.OrientationLocale : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"OrientationLocale","hash":{},"data":data}) : helper)))
    + "</span>\r\n        <span class=\"item-name\"><strong>"
    + escapeExpression(((helper = (helper = helpers.Title || (depth0 != null ? depth0.Title : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Title","hash":{},"data":data}) : helper)))
    + "</strong><br />"
    + escapeExpression(((helper = (helper = helpers.Name || (depth0 != null ? depth0.Name : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Name","hash":{},"data":data}) : helper)))
    + "</span>\r\n        <span class=\"item-date\">"
    + escapeExpression(((helper = (helper = helpers.CaptureDate_Short || (depth0 != null ? depth0.CaptureDate_Short : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"CaptureDate_Short","hash":{},"data":data}) : helper)))
    + "<br />"
    + escapeExpression(((helper = (helper = helpers.PublishDate_Short || (depth0 != null ? depth0.PublishDate_Short : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"PublishDate_Short","hash":{},"data":data}) : helper)))
    + "</span>\r\n        <div>\r\n            <i class=\"icon icon-asterisk "
    + escapeExpression(((helper = (helper = helpers.IsNewDim || (depth0 != null ? depth0.IsNewDim : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"IsNewDim","hash":{},"data":data}) : helper)))
    + "\" i18n-title=\"new\"></i>\r\n            <i class=\"icon icon-lock "
    + escapeExpression(((helper = (helper = helpers.IsPrivateDim || (depth0 != null ? depth0.IsPrivateDim : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"IsPrivateDim","hash":{},"data":data}) : helper)))
    + "\" i18n-title=\"private\"></i>\r\n            <i class=\"icon icon-feather "
    + escapeExpression(((helper = (helper = helpers.IsForStoryOnlyDim || (depth0 != null ? depth0.IsForStoryOnlyDim : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"IsForStoryOnlyDim","hash":{},"data":data}) : helper)))
    + "\" i18n-title=\"story-only\"></i>\r\n            <i class=\"icon icon-download "
    + escapeExpression(((helper = (helper = helpers.AllowFullSizeDownloadDim || (depth0 != null ? depth0.AllowFullSizeDownloadDim : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"AllowFullSizeDownloadDim","hash":{},"data":data}) : helper)))
    + "\" i18n-title=\"download-original\"></i>\r\n            <i class=\"icon icon-chat "
    + escapeExpression(((helper = (helper = helpers.AllowCommentingDim || (depth0 != null ? depth0.AllowCommentingDim : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"AllowCommentingDim","hash":{},"data":data}) : helper)))
    + "\" i18n-title=\"comments\"></i>\r\n            <i class=\"icon icon-star "
    + escapeExpression(((helper = (helper = helpers.AllowRatingDim || (depth0 != null ? depth0.AllowRatingDim : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"AllowRatingDim","hash":{},"data":data}) : helper)))
    + "\" i18n-title=\"rating\"></i>\r\n            <i class=\"icon icon-share "
    + escapeExpression(((helper = (helper = helpers.AllowSharingDim || (depth0 != null ? depth0.AllowSharingDim : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"AllowSharingDim","hash":{},"data":data}) : helper)))
    + "\" i18n-title=\"share\"></i>\r\n        </div>\r\n    </a>\r\n</li>";
},"useData":true});

this["fsTemplates"]["tPopoutAffirmation"] = Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "<div id=\"affirm-popout\" class=\"popout\">\r\n    <div class=\"popout-wrapper\">\r\n        <div class=\"popout-content\" style=\"width: 330px\">\r\n            <h3 data-i18n=\""
    + escapeExpression(((helper = (helper = helpers.TitleLocale || (depth0 != null ? depth0.TitleLocale : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TitleLocale","hash":{},"data":data}) : helper)))
    + "\"></h3>\r\n            <p data-i18n=\""
    + escapeExpression(((helper = (helper = helpers.MessageLocale || (depth0 != null ? depth0.MessageLocale : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"MessageLocale","hash":{},"data":data}) : helper)))
    + "\"></p>\r\n            <button id=\"affirm-ok\" class=\"expand\" data-i18n=\""
    + escapeExpression(((helper = (helper = helpers.OkLocale || (depth0 != null ? depth0.OkLocale : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"OkLocale","hash":{},"data":data}) : helper)))
    + "\"></button>\r\n        </div>\r\n    </div>\r\n</div>";
},"useData":true});

this["fsTemplates"]["tPopoutConfirmation"] = Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "<div id=\"confirm-popout\" class=\"popout\">\r\n    <div class=\"popout-wrapper\">\r\n        <div class=\"popout-content\" style=\"width: 330px\">\r\n            <h3 data-i18n=\""
    + escapeExpression(((helper = (helper = helpers.TitleLocale || (depth0 != null ? depth0.TitleLocale : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TitleLocale","hash":{},"data":data}) : helper)))
    + "\"></h3>\r\n            <p data-i18n=\""
    + escapeExpression(((helper = (helper = helpers.MessageLocale || (depth0 != null ? depth0.MessageLocale : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"MessageLocale","hash":{},"data":data}) : helper)))
    + "\"></p>\r\n            <div style=\"width: 272px; overflow-x: auto; overflow-y: hidden;\">\r\n                <button id=\"confirm-ok\" data-i18n=\""
    + escapeExpression(((helper = (helper = helpers.OkLocale || (depth0 != null ? depth0.OkLocale : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"OkLocale","hash":{},"data":data}) : helper)))
    + "\"></button>\r\n                <button id=\"confirm-cancel\" data-i18n=\""
    + escapeExpression(((helper = (helper = helpers.CancelLocale || (depth0 != null ? depth0.CancelLocale : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"CancelLocale","hash":{},"data":data}) : helper)))
    + "\"></button>                \r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>";
},"useData":true});

this["fsTemplates"]["tPopoutContent"] = Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression, buffer = "<div id=\"content-popout\" class=\"popout\">\r\n    <div class=\"popout-wrapper\" style=\"max-width: 100%; width: 100%;\">\r\n        <div class=\"popout-content\" style=\"width: 90%;\">\r\n            <a class=\"close\" href=\"javascript:void(0);\"><i class=\"icon icon-cancel-thin\"></i></a>\r\n            <h3 data-i18n=\""
    + escapeExpression(((helper = (helper = helpers.TitleLocale || (depth0 != null ? depth0.TitleLocale : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TitleLocale","hash":{},"data":data}) : helper)))
    + "\"></h3>\r\n            <div class=\"popout-content-inner\">\r\n                <div class=\"popout-content-wrapper\">\r\n                    ";
  stack1 = ((helper = (helper = helpers.Content || (depth0 != null ? depth0.Content : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Content","hash":{},"data":data}) : helper));
  if (stack1 != null) { buffer += stack1; }
  return buffer + "\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>";
},"useData":true});

this["fsTemplates"]["tRatingList"] = Handlebars.template({"1":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "    <a class=\"rater\" title=\""
    + escapeExpression(((helper = (helper = helpers.UserName || (depth0 != null ? depth0.UserName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"UserName","hash":{},"data":data}) : helper)))
    + "\"\r\n       style=\"background-image: url('"
    + escapeExpression(((helper = (helper = helpers.UserAvatarLink || (depth0 != null ? depth0.UserAvatarLink : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"UserAvatarLink","hash":{},"data":data}) : helper)))
    + "');\"\r\n       href=\""
    + escapeExpression(((helper = (helper = helpers.UserUrl || (depth0 != null ? depth0.UserUrl : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"UserUrl","hash":{},"data":data}) : helper)))
    + "/about\"></a>\r\n";
},"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, buffer = "<h3 data-i18n=\"rating\">Bewertungen</h3>\r\n<div class=\"rating-wrapper\">\r\n";
  stack1 = helpers.each.call(depth0, (depth0 != null ? depth0.Ratings : depth0), {"name":"each","hash":{},"fn":this.program(1, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer + "</div>\r\n";
},"useData":true});

this["fsTemplates"]["tSetup-DE"] = Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, buffer = "<div id=\"setup-intro\" class=\"page-details\">\r\n\r\n    <div class=\"details-commands\">\r\n        <a href=\"#\" onclick=\"global_auth_member.deleteAccount(); return false;\" data-i18n=\"delete-account\"><i class=\"icon icon-trash\"></i>Konto löschen</a>\r\n    </div>\r\n\r\n    <div class=\"details-panel\">\r\n\r\n        <h1>Willkommen bei FOTOSTEAM</h1>\r\n        <p>\r\n            Deine Registrierung war der erste Schritt zu Deinem Foto-Portfolio auf\r\n            FOTOSTEAM.\r\n        </p>\r\n        <p>\r\n            Der nächste ist die Verknüpfung Deines Konto mit Deinem Cloud-Anbieter, auf\r\n            dem alle Deine Fotos gespeichert werden, damit Du sie jederzeit im Zugriff\r\n            hast. Wähle aus...\r\n        </p>\r\n\r\n        <div id=\"setup-intro-provider\">\r\n";
  stack1 = this.invokePartial(partials.SetupProvider, '            ', 'SetupProvider', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  return buffer + "        </div>\r\n\r\n        <div id=\"setup-intro-info\">\r\n\r\n            <div id=\"setup-intro-info-provider-0\" class=\"setup-intro-info-provider\">\r\n                <p>\r\n                    FOTOSTEAM interagiert mit\r\n                    <a href=\"http://www.dropbox.com/\" target=\"_blank\"><strong>Dropbox</strong></a>\r\n                    als sogenannte Dropbox-App, d.h. unserem Dienst wird unter dem Ordner\r\n                    <span class=\"folder\">Apps</span> ein Ordner namens\r\n                    <span class=\"folder\">Fotosteam</span> zugewiesen, in dem wir Deine\r\n                    optimierten Bilder abgelegen. Wir haben nur Zugriff auf diesen Ordner.\r\n                </p>\r\n                <img class=\"screenshot\" src=\"/Content/Images/Screen-Windows-ToPublish-Dropbox.png\" />\r\n                <p>\r\n                    Am einfachsten bekommt Du neue Bilder auf Deine FOTOSTEAM-Seite, wenn Du sie in\r\n                    Deinen lokalen Order <span class=\"folder wrap\">.../Dropbox/App/Fotosteam/ToPublish</span>\r\n                    kopierst, wartest bis Dein Dropbox-Client alle Fotos hochgeladen hat und dann in\r\n                    Deinem Dashboard auf <strong>Synchronisieren</strong> klickst.\r\n                </p>\r\n                <p>\r\n                    Mit Dropbox hast Du zudem die Möglichkeit Deinen <span class=\"folder\">ToPublish</span>\r\n                    automatisch synchronisieren zu lassen. Dabei benachrichtigt Dropbox unseren Server,\r\n                    sobald dort eine neue Datei vorhanden ist und sie wird von uns automatisch verarbeitet.\r\n                    Dies kannst Du in den Dashboard-Optionen einschalten.\r\n                </p>\r\n                <hr />\r\n                <p>\r\n                    <strong>WICHTIG</strong>:<br />\r\n                    Bitte beachte gleich beim Verknüpfen auf Popup-Fenster Deines Browsers und lasse sie\r\n                    für dieser Seite zu!<br />\r\n                    Wenn es nicht geklappt haben sollte, lösche Dein Konto und leg es erneut an.\r\n                </p>\r\n            </div>\r\n\r\n            <div id=\"setup-intro-info-provider-1\" class=\"setup-intro-info-provider\">\r\n                <p>\r\n                    Bei <a href=\"http://drive.google.com/drive\" target=\"_blank\"><strong>Google Drive</strong></a>\r\n                    legen wir einen Ordner namens <span class=\"folder\">Fotosteam</span> an, in dem\r\n                    wir Deine optimierten Bilder abgelegen.\r\n                </p>\r\n                <p>\r\n                    Theoretisch haben wir Zugriff auf alle Deine Dateien auf Google Drive, da es dort\r\n                    kein App-Konzept gibt, aber wir kennen unsere Grenzen und werden niemals andere\r\n                    Dateien anfassen, außer Deinen FOTOSTEAM-Daten!\r\n                </p>\r\n                <img class=\"screenshot\" src=\"/Content/Images/Screen-Windows-ToPublish-GoogleDrive.png\" />\r\n                <p>\r\n                    Am einfachsten bekommt Du neue Bilder auf Deine FOTOSTEAM-Seite, wenn Du sie in\r\n                    Deinen lokalen Order <span class=\"folder wrap\">.../GoogleDrive/Fotosteam/ToPublish</span>\r\n                    kopierst, wartest bis Dein GoogleDrive-Client alle Fotos hochgeladen hat und dann in\r\n                    Deinem Dashboard auf <strong>Synchronisieren</strong> klickst.\r\n                </p>\r\n                <hr />\r\n                <p>\r\n                    <strong>WICHTIG</strong>:<br />\r\n                    Bitte beachte gleich beim Verknüpfen auf Popup-Fenster Deines Browsers und lasse sie\r\n                    für dieser Seite zu!<br />\r\n                    Wenn es nicht geklappt haben sollte, lösche Dein Konto und leg es erneut an.\r\n                </p>\r\n            </div>\r\n\r\n            <div id=\"setup-intro-info-provider-2\" class=\"setup-intro-info-provider\">\r\n                <p>\r\n                    <a href=\"http://onedrive.live.com/\" target=\"_blank\"><strong>Microsoft OneDrive</strong></a>\r\n                    legt bei der Verknüpfung mit dem FOTOSTEAM-Konto im Ordner\r\n                    <span class=\"folder\">Anwendungen</span> einen Ordner namens\r\n                    <span class=\"folder\">Fotosteam</span> an, in dem wir Deine optimierten\r\n                    Bilder abgelegen. Wir haben nur Zugriff auf diesen Ordner.\r\n                </p>\r\n                <img class=\"screenshot\" src=\"/Content/Images/Screen-Windows-ToPublish-OneDrive.png\" />\r\n                <p>\r\n                    Am einfachsten bekommt Du neue Bilder auf Deine FOTOSTEAM-Seite, wenn Du sie in\r\n                    Deinen lokalen Order <span class=\"folder wrap\">.../OneDrive/Anwendungen/Fotosteam/ToPublish</span>\r\n                    kopierst, wartest bis Dein OneDrive-Client alle Fotos hochgeladen hat und dann in Deinem\r\n                    Dashboard auf <strong>Synchronisieren</strong> klickst.\r\n                </p>\r\n                <hr />\r\n                <p>\r\n                    <strong>WICHTIG</strong>:<br />\r\n                    Bitte beachte gleich beim Verknüpfen auf Popup-Fenster Deines Browsers und lasse sie\r\n                    für dieser Seite zu!<br />\r\n                    Wenn es nicht geklappt haben sollte, lösche Dein Konto und leg es erneut an.\r\n                </p>\r\n            </div>\r\n\r\n        </div>\r\n\r\n        <button id=\"setup-intro-command\" class=\"expand\" disabled=\"disabled\">(Bitte wähle Deinen Cloud-Provider aus)</button>\r\n\r\n    </div>\r\n\r\n</div>";
},"usePartial":true,"useData":true});

this["fsTemplates"]["tSetup-EN"] = Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, buffer = "<div id=\"setup-intro\" class=\"page-details\">\r\n\r\n    <div class=\"details-commands\">\r\n        <a href=\"#\" onclick=\"global_auth_member.deleteAccount(); return false;\"\r\n           data-i18n=\"delete-account\"><i class=\"icon icon-trash\"></i>Delete Account</a>\r\n    </div>\r\n\r\n    <div class=\"details-panel\">\r\n\r\n        <h1>Welcome to FOTOSTEAM</h1>\r\n        <p>\r\n            Your registration was the first step to your photo portfolio on\r\n            FOTOSTEAM.\r\n        </p>\r\n        <p>\r\n            The next one is linking your account to your cloud provider,\r\n            where alle your photo will be stored, to access them easily. Please choose...\r\n        </p>\r\n        <div id=\"setup-intro-provider\">\r\n";
  stack1 = this.invokePartial(partials.SetupProvider, '            ', 'SetupProvider', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  return buffer + "        </div>\r\n\r\n        <div id=\"setup-intro-info\">\r\n\r\n            <div id=\"setup-intro-info-provider-0\" class=\"setup-intro-info-provider\">\r\n                <p>\r\n                    FOTOSTEAM interacts with\r\n                    <a href=\"http://www.dropbox.com/\" target=\"_blank\"><strong>Dropbox</strong></a>\r\n                    as a so-called Dropbox-App, i.e. we have our own folder named\r\n                    <span class=\"folder\">Fotosteam</span> under the folder\r\n                    <span class=\"folder\">Apps</span>, where we will store\r\n                    your optimized photos. We have only access to this folder.\r\n                </p>\r\n                <img class=\"screenshot\" src=\"/Content/Images/Screen-Windows-ToPublish-Dropbox.png\" />\r\n                <p>\r\n                    Most easiest way to get your photos on your FOTOSTEAM page is, if you copy them to\r\n                    your local folder <span class=\"folder wrap\">.../Dropbox/App/Fotosteam/ToPublish</span>,\r\n                    wait until your Dropbox client has uploaded all files and then click on the button\r\n                    <strong>Synchronize</strong> on your dashboard.\r\n                </p>\r\n                <p>\r\n                    With Dropbox you have furthermore the possibility to enable automatic synchronization\r\n                    of your <span class=\"folder\">ToPublish</span>folder. Thereby Dropbox is notifying our\r\n                    server, if a new file is present and we will process it automatically. For that you find\r\n                    a switch in your dashboard options.\r\n                </p>\r\n                <hr />\r\n                <p>\r\n                    <strong>Important</strong>:<br />\r\n                    For linking correctly to your provider, please beware of your browsers popup windows\r\n                    and allow them for this site!<br />\r\n                    In case it went wrong, delete your account and create a new one.\r\n                </p>\r\n            </div>\r\n\r\n            <div id=\"setup-intro-info-provider-1\" class=\"setup-intro-info-provider\">\r\n                <p>\r\n                    At <a href=\"http://drive.google.com/drive\" target=\"_blank\"><strong>Google Drive</strong></a>\r\n                    we create a new folder named <span class=\"folder\">Fotosteam</span>, where we\r\n                    will store your optimized photos.\r\n                </p>\r\n                <p>\r\n                    Theoretically, we have access to all of your files on Google Drive, because they\r\n                    have no concept for apps, but we know our bounds and will never touch any files\r\n                    accept your FOTOSTEAM data!\r\n                </p>\r\n                <img class=\"screenshot\" src=\"/Content/Images/Screen-Windows-ToPublish-GoogleDrive.png\" />\r\n                <p>\r\n                    Most easiest way to get your photos on your FOTOSTEAM page is, if you copy them to\r\n                    your local folder <span class=\"folder wrap\">.../GoogleDrive/Fotosteam/ToPublish</span>,\r\n                    wait until your GoogleDrive client has uploaded all files and then click on the button\r\n                    <strong>Synchronize</strong> on your dashboard.\r\n                </p>\r\n                <hr />\r\n                <p>\r\n                    <strong>Important</strong>:<br />\r\n                    For linking correctly to your provider, please beware of your browsers popup windows\r\n                    and allow them for this site!<br />\r\n                    In case it went wrong, delete your account and create a new one.\r\n                </p>\r\n            </div>\r\n\r\n            <div id=\"setup-intro-info-provider-2\" class=\"setup-intro-info-provider\">\r\n                <p>\r\n                    <a href=\"http://onedrive.live.com/\" target=\"_blank\"><strong>Microsoft OneDrive</strong></a>\r\n                    creates, while linking with your FOTOSTEAM account, a new folder named\r\n                    <span class=\"folder\">Fotosteam</span> below the folder\r\n                    <span class=\"folder\">Anwendungen</span>, where we will store\r\n                    your optimized photos. We have only access to this folder.\r\n                </p>\r\n                <img class=\"screenshot\" src=\"/Content/Images/Screen-Windows-ToPublish-OneDrive.png\" />\r\n                <p>\r\n                    Most easiest way to get your photos on your FOTOSTEAM page is, if you copy them to\r\n                    your local folder <span class=\"folder wrap\">.../OneDrive/Anwendungen/Fotosteam/ToPublish</span>,\r\n                    wait until your OneDrive client has uploaded all files and then click on the button\r\n                    <strong>Synchronize</strong> on your dashboard.\r\n                </p>\r\n                <hr />\r\n                <p>\r\n                    <strong>Important</strong>:<br />\r\n                    For linking correctly to your provider, please beware of your browsers popup windows\r\n                    and allow them for this site!<br />\r\n                    In case it went wrong, delete your account and create a new one.\r\n                </p>\r\n            </div>\r\n\r\n        </div>\r\n\r\n        <button id=\"setup-intro-command\" class=\"expand\" disabled=\"disabled\">(Please choose your cloud provider)</button>\r\n\r\n    </div>\r\n\r\n</div>";
},"usePartial":true,"useData":true});

this["fsTemplates"]["tSidebarCommon"] = Handlebars.template({"1":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = this.invokePartial(partials.LoginBay, '    ', 'LoginBay', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"3":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = this.invokePartial(partials.RegisterBay, '    ', 'RegisterBay', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"5":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = this.invokePartial(partials.NotificationBay, '    ', 'NotificationBay', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, buffer = "<div class=\"sidebar-wrapper\">\r\n\r\n    <div class=\"head\">\r\n        <a href=\"/start\"><h2>Fotosteam</h2></a>\r\n    </div>\r\n\r\n";
  stack1 = helpers.unless.call(depth0, (depth0 != null ? depth0.AuthMember : depth0), {"name":"unless","hash":{},"fn":this.program(1, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = helpers.unless.call(depth0, (depth0 != null ? depth0.Registered : depth0), {"name":"unless","hash":{},"fn":this.program(3, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.AuthMember : depth0), {"name":"if","hash":{},"fn":this.program(5, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = this.invokePartial(partials.ContactBay, '    ', 'ContactBay', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = this.invokePartial(partials.KeyboardBay, '    ', 'KeyboardBay', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n    <ul class=\"sidebar-commands\">\r\n\r\n";
  stack1 = this.invokePartial(partials.SidebarCommonCommands, '        ', 'SidebarCommonCommands', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  return buffer + "\r\n    </ul>\r\n\r\n</div>";
},"usePartial":true,"useData":true});

this["fsTemplates"]["tSidebarMember"] = Handlebars.template({"1":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                <li>\r\n                    <a class=\"icon icon-pin\" href=\""
    + escapeExpression(((helper = (helper = helpers.CategoryUrl || (depth0 != null ? depth0.CategoryUrl : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"CategoryUrl","hash":{},"data":data}) : helper)))
    + "\">\r\n                        "
    + escapeExpression(((helper = (helper = helpers.CategoryName || (depth0 != null ? depth0.CategoryName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"CategoryName","hash":{},"data":data}) : helper)))
    + "<i>"
    + escapeExpression(((helper = (helper = helpers.CategoryPhotoCount || (depth0 != null ? depth0.CategoryPhotoCount : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"CategoryPhotoCount","hash":{},"data":data}) : helper)))
    + "</i>\r\n                    </a>\r\n                </li>\r\n";
},"3":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, blockHelperMissing=helpers.blockHelperMissing, buffer = "            <ul>\r\n";
  stack1 = blockHelperMissing.call(depth0, lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.TopicsUsed : stack1), depth0), {"name":"Member.TopicsUsed","hash":{},"fn":this.program(4, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer + "            </ul>\r\n";
},"4":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                <li>\r\n                    <a class=\"icon icon-bullseye\" href=\""
    + escapeExpression(((helper = (helper = helpers.TopicUrl || (depth0 != null ? depth0.TopicUrl : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TopicUrl","hash":{},"data":data}) : helper)))
    + "\">\r\n                        "
    + escapeExpression(((helper = (helper = helpers.TopicName || (depth0 != null ? depth0.TopicName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TopicName","hash":{},"data":data}) : helper)))
    + "<i>"
    + escapeExpression(((helper = (helper = helpers.TopicPhotoCount || (depth0 != null ? depth0.TopicPhotoCount : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"TopicPhotoCount","hash":{},"data":data}) : helper)))
    + "</i>\r\n                    </a>\r\n                </li>\r\n";
},"6":function(depth0,helpers,partials,data) {
  return "            <p data-i18n=\"not-available-yet\">Noch keine vorhanden</p>\r\n";
  },"8":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, blockHelperMissing=helpers.blockHelperMissing, buffer = "            <ul>\r\n";
  stack1 = blockHelperMissing.call(depth0, lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.LocationGroups : stack1), depth0), {"name":"Member.LocationGroups","hash":{},"fn":this.program(9, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer + "            </ul>\r\n";
},"9":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                <li>\r\n                    <a class=\"icon icon-location\" href=\""
    + escapeExpression(((helper = (helper = helpers.LocationUrl || (depth0 != null ? depth0.LocationUrl : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationUrl","hash":{},"data":data}) : helper)))
    + "\">\r\n                        "
    + escapeExpression(((helper = (helper = helpers.LocationName || (depth0 != null ? depth0.LocationName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationName","hash":{},"data":data}) : helper)))
    + "<i>"
    + escapeExpression(((helper = (helper = helpers.LocationPhotoCount || (depth0 != null ? depth0.LocationPhotoCount : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"LocationPhotoCount","hash":{},"data":data}) : helper)))
    + "</i>\r\n                    </a>\r\n                </li>\r\n";
},"11":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, blockHelperMissing=helpers.blockHelperMissing, buffer = "            <ul>\r\n";
  stack1 = blockHelperMissing.call(depth0, lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.EventsUsed : stack1), depth0), {"name":"Member.EventsUsed","hash":{},"fn":this.program(12, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer + "            </ul>\r\n";
},"12":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                <li>\r\n                    <a class=\"icon icon-rocket\" href=\""
    + escapeExpression(((helper = (helper = helpers.EventUrl || (depth0 != null ? depth0.EventUrl : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"EventUrl","hash":{},"data":data}) : helper)))
    + "\">\r\n                        "
    + escapeExpression(((helper = (helper = helpers.EventCaption || (depth0 != null ? depth0.EventCaption : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"EventCaption","hash":{},"data":data}) : helper)))
    + "<i>"
    + escapeExpression(((helper = (helper = helpers.EventPhotoCount || (depth0 != null ? depth0.EventPhotoCount : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"EventPhotoCount","hash":{},"data":data}) : helper)))
    + "</i>\r\n                    </a>\r\n                </li>\r\n";
},"14":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, blockHelperMissing=helpers.blockHelperMissing, buffer = "            <ul>\r\n";
  stack1 = blockHelperMissing.call(depth0, lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Stories : stack1), depth0), {"name":"Member.Stories","hash":{},"fn":this.program(15, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer + "            </ul>\r\n";
},"15":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "                <li>\r\n                    <a class=\"icon icon-feather\" href=\""
    + escapeExpression(((helper = (helper = helpers.StoryUrl || (depth0 != null ? depth0.StoryUrl : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"StoryUrl","hash":{},"data":data}) : helper)))
    + "\">\r\n                        "
    + escapeExpression(((helper = (helper = helpers.StoryName || (depth0 != null ? depth0.StoryName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"StoryName","hash":{},"data":data}) : helper)))
    + "<i>"
    + escapeExpression(((helper = (helper = helpers.StoryPhotoCount || (depth0 != null ? depth0.StoryPhotoCount : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"StoryPhotoCount","hash":{},"data":data}) : helper)))
    + "</i>\r\n                    </a>\r\n                </li>\r\n";
},"17":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = this.invokePartial(partials.LoginBay, '        ', 'LoginBay', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"19":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = this.invokePartial(partials.RegisterBay, '        ', 'RegisterBay', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"21":function(depth0,helpers,partials,data) {
  var stack1, buffer = "";
  stack1 = this.invokePartial(partials.NotificationBay, '        ', 'NotificationBay', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, lambda=this.lambda, escapeExpression=this.escapeExpression, blockHelperMissing=helpers.blockHelperMissing, buffer = "<div class=\"sidebar-wrapper\">\r\n    \r\n    <div class=\"head\">        \r\n        <a href=\"/start\"><h2>Fotosteam</h2></a>\r\n        <small class=\"head-by\">by</small>\r\n        <a class=\"head-steam\" href=\"/"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Steam : stack1), depth0))
    + "/about\">"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.PlainName : stack1), depth0))
    + "</a>\r\n    </div>\r\n\r\n    <div id=\"sidebar-bay-categories\" class=\"sidebar-bay\">\r\n        <div id=\"bay-categories\" class=\"bay-wrapper\">\r\n            <a class=\"bay-close icon icon-angle-left\" onclick=\"closeSidebarBays();\"\r\n               data-i18n=\"categories\">Kategorien</a>            \r\n            <ul>\r\n";
  stack1 = blockHelperMissing.call(depth0, lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Categories : stack1), depth0), {"name":"Member.Categories","hash":{},"fn":this.program(1, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "            </ul>\r\n        </div>\r\n    </div>\r\n    <div id=\"sidebar-bay-topics\" class=\"sidebar-bay\">\r\n        <div id=\"bay-topics\" class=\"bay-wrapper\">\r\n            <a class=\"bay-close icon icon-angle-left\" onclick=\"closeSidebarBays();\"\r\n               data-i18n=\"topics\">Themen</a>\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Topics : stack1)) != null ? stack1.length : stack1), {"name":"if","hash":{},"fn":this.program(3, data),"inverse":this.program(6, data),"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "        </div>\r\n    </div>\r\n    <div id=\"sidebar-bay-locations\" class=\"sidebar-bay\">\r\n        <div id=\"bay-locations\" class=\"bay-wrapper\">\r\n            <a class=\"bay-close icon icon-angle-left\" onclick=\"closeSidebarBays();\"\r\n               data-i18n=\"locations\">Orte</a>\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.LocationGroups : stack1)) != null ? stack1.length : stack1), {"name":"if","hash":{},"fn":this.program(8, data),"inverse":this.program(6, data),"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "        </div>\r\n    </div>\r\n    <div id=\"sidebar-bay-events\" class=\"sidebar-bay\">\r\n        <div id=\"bay-events\" class=\"bay-wrapper\">\r\n            <a class=\"bay-close icon icon-angle-left\" onclick=\"closeSidebarBays();\"\r\n               data-i18n=\"events\">Veranstaltungen</a>\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Events : stack1)) != null ? stack1.length : stack1), {"name":"if","hash":{},"fn":this.program(11, data),"inverse":this.program(6, data),"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "        </div>\r\n    </div>\r\n    <div id=\"sidebar-bay-stories\" class=\"sidebar-bay\">\r\n        <div id=\"bay-stories\" class=\"bay-wrapper\">\r\n            <a class=\"bay-close icon icon-angle-left\" onclick=\"closeSidebarBays();\"\r\n               data-i18n=\"stories\">Geschichten</a>\r\n";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Stories : stack1)) != null ? stack1.length : stack1), {"name":"if","hash":{},"fn":this.program(14, data),"inverse":this.program(6, data),"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "        </div>\r\n    </div>\r\n    \r\n";
  stack1 = helpers.unless.call(depth0, (depth0 != null ? depth0.AuthMember : depth0), {"name":"unless","hash":{},"fn":this.program(17, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = helpers.unless.call(depth0, (depth0 != null ? depth0.Registered : depth0), {"name":"unless","hash":{},"fn":this.program(19, data),"inverse":this.program(21, data),"data":data});
  if (stack1 != null) { buffer += stack1; }
  buffer += "    \r\n";
  stack1 = this.invokePartial(partials.ContactBay, '    ', 'ContactBay', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n";
  stack1 = this.invokePartial(partials.KeyboardBay, '    ', 'KeyboardBay', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  buffer += "    \r\n    <ul class=\"sidebar-commands\">\r\n        <li>\r\n            <a class=\"cmd-journal\" href=\"/"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Steam : stack1), depth0))
    + "/journal\">\r\n                <i class=\"icon icon-history\"></i>\r\n                <span data-i18n=\"journal\">Journal</span>\r\n            </a>\r\n        </li>\r\n        <li>\r\n            <a class=\"cmd-overview\" href=\"/"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.Member : depth0)) != null ? stack1.Steam : stack1), depth0))
    + "/overview\">\r\n                <i class=\"icon icon-grid-large\" style=\"font-size: 0.6em;\"></i>\r\n                <span data-i18n=\"overview\">Übersicht</span>\r\n            </a>\r\n        </li>\r\n        <li>\r\n            <a class=\"cmd-categories cmd-bay\" href=\"#\" data-bay=\"categories\">\r\n                <i class=\"icon icon-pin\"></i>\r\n                <span data-i18n=\"categories\">Kategorien</span>\r\n            </a>\r\n        </li>\r\n        <li>\r\n            <a class=\"cmd-topics cmd-bay\" href=\"#\" data-bay=\"topics\">\r\n                <i class=\"icon icon-bullseye\"></i>\r\n                <span data-i18n=\"topics\">Themen</span>\r\n            </a>\r\n        </li>\r\n        <li>\r\n            <a class=\"cmd-locations cmd-bay\" href=\"#\" data-bay=\"locations\">\r\n                <i class=\"icon icon-location\"></i>\r\n                <span data-i18n=\"locations\">Orte</span>\r\n            </a>\r\n        </li>\r\n        <li>\r\n            <a class=\"cmd-events cmd-bay\" href=\"#\" data-bay=\"events\">\r\n                <i class=\"icon icon-rocket\"></i>\r\n                <span data-i18n=\"events\">Veranstaltungen</span>\r\n            </a>\r\n        </li>\r\n        <li>\r\n            <a class=\"cmd-stories cmd-bay\" href=\"#\" data-bay=\"stories\">\r\n                <i class=\"icon icon-feather\"></i>\r\n                <span data-i18n=\"stories\">Geschichten</span>\r\n            </a>\r\n        </li>\r\n        \r\n        <li class=\"line\"></li>\r\n\r\n";
  stack1 = this.invokePartial(partials.SidebarCommonCommands, '        ', 'SidebarCommonCommands', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  return buffer + "\r\n    </ul>\r\n\r\n\r\n</div>\r\n";
},"usePartial":true,"useData":true});

this["fsTemplates"]["tStart-DE"] = Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, buffer = "<div id=\"start-intro\" class=\"box\">\r\n\r\n";
  stack1 = this.invokePartial(partials.StartScreenshot, '    ', 'StartScreenshot', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n    <div id=\"start-intro-text\">\r\n        <h1>Ein Platz für Deine Fotos</h1>\r\n\r\n        <p class=\"subtitle\" style=\"margin-bottom: 30px;\">\r\n            FOTOSTEAM ist die neue Art des Foto-Portfolios.\r\n            Präsentiere Deine Bilder auf die einzige Art, die ihnen gebührt:<br />\r\n            In voller Breite und ohne Kompromisse. Jeder Zoll für Deine Arbeit.\r\n        </p>\r\n\r\n";
  stack1 = this.invokePartial(partials.StartFeatures, '        ', 'StartFeatures', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  stack1 = this.invokePartial(partials.StartIntroButtons, '        ', 'StartIntroButtons', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  buffer += "    </div>\r\n</div>\r\n\r\n<div id=\"start-howto\" class=\"box\" style=\"display:none;\">\r\n\r\n    <div id=\"start-howto-text\">\r\n        <h1>So funktioniert FOTOSTEAM</h1>\r\n\r\n        <p>Zunächst einmal...</p>\r\n        <p class=\"subtitle\">\r\n            Deine Daten bleiben wo sie hingehören: bei Dir!\r\n        </p>\r\n        <p>\r\n            FOTOSTEAM, speichert Deine Fotos auf Deinem Cloud-Anbieter und\r\n            nicht auf irgendeinem Server, wo Du keinen direkten Zugriff darauf\r\n            hast. Dadurch behälst Du die volle und einfache Kontrolle über Deine Daten!\r\n        </p>\r\n\r\n        <h3 class=\"strip\">Schritt 1</h3>\r\n        <p>\r\n            Du legst Dir ein <strong>FOTOSTEAM-Konto</strong> an, indem Du Dich über einen\r\n            der folgenden Social-Media-Dienste <strong>registrierst</strong>, denn wir speichern\r\n            auch Dein Kennwort nicht...\r\n        </p>\r\n        <p>\r\n";
  stack1 = this.invokePartial(partials.StartAuthBrand, '            ', 'StartAuthBrand', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  buffer += "        </p>\r\n\r\n        <h3 class=\"strip\">Schritt 2</h3>\r\n        <p>\r\n            Du verknüpfst Dein Konto mit einem der verfügbaren <strong>Cloud-Anbieter</strong>...\r\n        </p>\r\n";
  stack1 = this.invokePartial(partials.StartCloudBrand, '        ', 'StartCloudBrand', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n        <h3 class=\"strip\">Schritt 3</h3>\r\n        <p>\r\n            Du lädst Deine Fotos entweder über unsere Web-Oberfläche hoch, oder Du kopierst\r\n            sie einfach in den Ordner <span class=\"folder\">.../Fotosteam/ToPublish</span> auf\r\n            Deiner Festplatte, der bei der Verlinkung mit Deinem Cloud-Anbieter erzeugt wurde.\r\n        </p>\r\n        <img src=\"/Content/Images/Screen-Windows-ToPublish-Dropbox.png\" style=\"display:block; margin: 10px 0;\" />\r\n\r\n        <h3 class=\"strip\">Schritt 4</h3>\r\n        <p>\r\n            Synchronisiere Deinen Fotos über die Schaltfläche auf Deinem\r\n            <a data-i18n=\"dashboard\" href=\"/dashboard\">Dashboard</a>. Dropbox-Benutzer\r\n            können sich diesen Schritt sparen, wenn sie in ihren\r\n            <a data-i18n=\"dashboard\" href=\"/dashboard#options\">Optionen</a> die\r\n            <strong>automatische Synchronisierung</strong> einschalten.\r\n        </p>\r\n        <p>\r\n            Die Bilder werden dabei aus dem ToPublish-Ordner ausgelesen, optimiert und in verschiedenen Größen\r\n            in einem neuen Ordner unter <span class=\"folder\">.../Fotosteam/Internal/Fotos</span> abgelegt, um Deinen\r\n            Freunden das bestmögliche Fotoerlebnis zu bieten.\r\n        </p>\r\n        <img src=\"/Content/Images/Screen-Windows-Foto-Dropbox.png\" style=\"display:block; margin: 10px 0;\" />\r\n\r\n        <h3 class=\"strip\">Schritt 5</h3>\r\n        <p>\r\n            Nach der Synchronisierung findest Du alle Deine neuen Fotos auf dem Dashboard-Karteireiter\r\n            <strong>Neue Fotos</strong>.\r\n        </p>\r\n        <p><strong>Fertig!</strong></p>\r\n\r\n    </div>\r\n    <div id=\"start-howto-pic\"></div>\r\n\r\n</div>\r\n\r\n";
  stack1 = this.invokePartial(partials.StartFotosNew, '', 'StartFotosNew', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  stack1 = this.invokePartial(partials.StartFotosTop, '', 'StartFotosTop', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n<div id=\"start-featured-story\" class=\"box\">\r\n    <div id=\"start-featured-story-text\">\r\n        <h1>Empfohlene Geschichte</h1>\r\n        <p>\r\n            <strong>FOTOSTEAM Geschichten</strong> werden in wenigen Wochen fertiggestellt und\r\n            eingeführt. Um Dir zu zeigen wie diese aussehen, haben wir uns entschlossen eine\r\n            Vorversion aufzusetzen und Dir die Geschichte eine Wanderung zu erzählen ...\r\n            einer langen Wanderung.\r\n        </p>\r\n        <p>\r\n            <a href=\"/robert\">Robert</a>, Mitgründer von FOTOSTEAM, ist aktuell auf einer Tour\r\n            entlang des <a href=\"http://www.wikiwand.com/en/Appalachian_Trail\" target=\"_blank\">Appalachian Trail</a>\r\n            und berichtet über dieses Abenteuer, wann immer er Zugnag zum Internet hat.\r\n        </p>\r\n        <p>\r\n            Stay tuned...\r\n        </p>\r\n    </div>\r\n    <div id=\"start-featured-story-pic\"\r\n         style=\"background-image: url(https://dl.dropboxusercontent.com/s/vx0lr89jiw8ebzb/640.jpg);\">\r\n        <a href=\"/robert/story/appalachian-trail\">Appalachian Trail<span>von Robert Bakos</span></a>\r\n    </div>\r\n</div>\r\n\r\n\r\n";
  stack1 = this.invokePartial(partials.StartMember, '', 'StartMember', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"usePartial":true,"useData":true});

this["fsTemplates"]["tStart-EN"] = Handlebars.template({"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, buffer = "<div id=\"start-intro\" class=\"box\">\r\n\r\n";
  stack1 = this.invokePartial(partials.StartScreenshot, '    ', 'StartScreenshot', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n    <div id=\"start-intro-text\">\r\n        <h1>A place for your photos</h1>\r\n\r\n        <p class=\"subtitle\" style=\"margin-bottom: 30px;\">\r\n            FOTOSTEAM is a new kind of photo portfolio.\r\n            Present your images the only way they deserve:<br />\r\n            In full width and uncompromising. Every inch for your work.\r\n        </p>\r\n\r\n";
  stack1 = this.invokePartial(partials.StartFeatures, '        ', 'StartFeatures', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  stack1 = this.invokePartial(partials.StartIntroButtons, '        ', 'StartIntroButtons', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  buffer += "    </div>\r\n</div>\r\n\r\n<div id=\"start-howto\" class=\"box\" style=\"display:none;\">\r\n\r\n    <div id=\"start-howto-text\">\r\n        <h1>How FOTOSTEAM works</h1>\r\n\r\n        <p>First of all...</p>\r\n        <p class=\"subtitle\">\r\n            Your data stay where they belong: with you!\r\n        </p>\r\n        <p>\r\n            FOTOSTEAM, saves your photos at your cloud provider and\r\n            not on a server, where you don't have direct access.\r\n            This way, you keep full and easy control over your data!\r\n        </p>\r\n\r\n        <h3 class=\"strip\">Step 1</h3>\r\n        <p>\r\n            Create your <strong>FOTOSTEAM</strong> account, by\r\n            <strong>registering</strong> up via one of the following\r\n            social media services, because, neither we store your\r\n            password...\r\n        </p>\r\n        <p>\r\n";
  stack1 = this.invokePartial(partials.StartAuthBrand, '            ', 'StartAuthBrand', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  buffer += "        </p>\r\n\r\n        <h3 class=\"strip\">Step 2</h3>\r\n        <p>\r\n            You connect your account with one of the available cloud providers...\r\n        </p>\r\n";
  stack1 = this.invokePartial(partials.StartCloudBrand, '        ', 'StartCloudBrand', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n        <h3 class=\"strip\">Step 3</h3>\r\n        <p>\r\n            You upload your photos either via this website or you just copy them in the folder\r\n            named <span class=\"folder\">.../Fotosteam/ToPublish</span> on your hard-drive,\r\n            which was created during the connect with your cloud provider.\r\n        </p>\r\n        <img src=\"/Content/Images/Screen-Windows-ToPublish-Dropbox.png\" style=\"display:block; margin: 10px 0;\" />\r\n\r\n        <h3 class=\"strip\">Step 4</h3>\r\n        <p>\r\n            Synchronize the photos via the button on your\r\n            <a data-i18n=\"dashboard\" href=\"/dashboard\">Dashboard</a>.\r\n            Dropbox users can save the effort by activating\r\n            <strong>automatic sync</strong> in their\r\n            <a data-i18n=\"dashboard\" href=\"/dashboard#options\">options</a>.\r\n        </p>\r\n        <p>\r\n            The images will be read from the ToPublish-folder, optimized and stored in several sizes in a new folder under\r\n            <span class=\"folder\">.../Fotosteam/Internal/Fotos</span>, for giving your friends the best\r\n            photo experience.\r\n        </p>\r\n        <img src=\"/Content/Images/Screen-Windows-Foto-Dropbox.png\" style=\"display:block; margin: 10px 0;\" />\r\n\r\n        <h3 class=\"strip\">Step 5</h3>\r\n        <p>\r\n            After the synchronization you will find your new photos on the dashboard tab <strong>New Photos</strong>.\r\n        </p>\r\n        <p><strong>Done!</strong></p>\r\n\r\n    </div>\r\n    <div id=\"start-howto-pic\"></div>\r\n\r\n</div>\r\n\r\n";
  stack1 = this.invokePartial(partials.StartFotosNew, '', 'StartFotosNew', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  stack1 = this.invokePartial(partials.StartFotosTop, '', 'StartFotosTop', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  buffer += "\r\n<div id=\"start-featured-story\" class=\"box\">\r\n    <div id=\"start-featured-story-text\">\r\n        <h1>Featured Story</h1>\r\n        <p>\r\n            <strong>FOTOSTEAM Stories</strong> will be finalized and introduced in a\r\n            few weeks. To show you how they look like, we decided to set up\r\n            a pre-release and tell you the story about a walk ... a long walk.\r\n        </p>\r\n        <p>\r\n            <a href=\"/robert\">Robert</a>, co-founder of FOTOSTEAM, is actually on a 5 month\r\n            hike alongside the <a href=\"http://www.wikiwand.com/en/Appalachian_Trail\" target=\"_blank\">Appalachian Trail</a>\r\n            and is reporting about this adventure, whenever he has access to the internet.\r\n        </p>\r\n        <p>\r\n            Stay tuned...\r\n        </p>\r\n    </div>\r\n    <div id=\"start-featured-story-pic\"\r\n         style=\"background-image: url(https://dl.dropboxusercontent.com/s/vx0lr89jiw8ebzb/640.jpg);\">\r\n        <a href=\"/robert/story/appalachian-trail\">Appalachian Trail<span>by Robert Bakos</span></a>\r\n    </div>\r\n</div>\r\n\r\n";
  stack1 = this.invokePartial(partials.StartMember, '', 'StartMember', depth0, undefined, helpers, partials, data);
  if (stack1 != null) { buffer += stack1; }
  return buffer;
},"usePartial":true,"useData":true});

this["fsTemplates"]["tStory"] = Handlebars.template({"1":function(depth0,helpers,partials,data,depths) {
  var stack1, helper, options, functionType="function", helperMissing=helpers.helperMissing, blockHelperMissing=helpers.blockHelperMissing, lambda=this.lambda, escapeExpression=this.escapeExpression, buffer = "    <div class=\"chapter\">\r\n        <figcaption>\r\n            \r\n";
  stack1 = ((helper = (helper = helpers.PrevChapters || (depth0 != null ? depth0.PrevChapters : depth0)) != null ? helper : helperMissing),(options={"name":"PrevChapters","hash":{},"fn":this.program(2, data, depths),"inverse":this.noop,"data":data}),(typeof helper === functionType ? helper.call(depth0, options) : helper));
  if (!helpers.PrevChapters) { stack1 = blockHelperMissing.call(depth0, stack1, options); }
  if (stack1 != null) { buffer += stack1; }
  buffer += "            \r\n            <span style=\"margin:0 10px;\">"
    + escapeExpression(lambda(((stack1 = (depth0 != null ? depth0.CurrentChapter : depth0)) != null ? stack1.ChapterName : stack1), depth0))
    + "</span>\r\n\r\n";
  stack1 = ((helper = (helper = helpers.NextChapters || (depth0 != null ? depth0.NextChapters : depth0)) != null ? helper : helperMissing),(options={"name":"NextChapters","hash":{},"fn":this.program(4, data, depths),"inverse":this.noop,"data":data}),(typeof helper === functionType ? helper.call(depth0, options) : helper));
  if (!helpers.NextChapters) { stack1 = blockHelperMissing.call(depth0, stack1, options); }
  if (stack1 != null) { buffer += stack1; }
  buffer += "        </figcaption>\r\n\r\n        <div class=\"ledges\"></div>\r\n\r\n        <div class=\"chapters content-box\">\r\n            <div class=\"prev-chapter "
    + escapeExpression(((helper = (helper = helpers.PrevChapterClass || (depth0 != null ? depth0.PrevChapterClass : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"PrevChapterClass","hash":{},"data":data}) : helper)))
    + "\">\r\n";
  stack1 = ((helper = (helper = helpers.PrevChapter || (depth0 != null ? depth0.PrevChapter : depth0)) != null ? helper : helperMissing),(options={"name":"PrevChapter","hash":{},"fn":this.program(6, data, depths),"inverse":this.noop,"data":data}),(typeof helper === functionType ? helper.call(depth0, options) : helper));
  if (!helpers.PrevChapter) { stack1 = blockHelperMissing.call(depth0, stack1, options); }
  if (stack1 != null) { buffer += stack1; }
  buffer += "            </div>\r\n            <div class=\"next-chapter "
    + escapeExpression(((helper = (helper = helpers.NextChapterClass || (depth0 != null ? depth0.NextChapterClass : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"NextChapterClass","hash":{},"data":data}) : helper)))
    + "\">\r\n";
  stack1 = ((helper = (helper = helpers.NextChapter || (depth0 != null ? depth0.NextChapter : depth0)) != null ? helper : helperMissing),(options={"name":"NextChapter","hash":{},"fn":this.program(8, data, depths),"inverse":this.noop,"data":data}),(typeof helper === functionType ? helper.call(depth0, options) : helper));
  if (!helpers.NextChapter) { stack1 = blockHelperMissing.call(depth0, stack1, options); }
  if (stack1 != null) { buffer += stack1; }
  return buffer + "            </div>\r\n        </div>\r\n    </div>\r\n";
},"2":function(depth0,helpers,partials,data,depths) {
  var stack1, helper, lambda=this.lambda, escapeExpression=this.escapeExpression, functionType="function", helperMissing=helpers.helperMissing;
  return "            <a class=\"leftwing\" href=\""
    + escapeExpression(lambda(((stack1 = (depths[1] != null ? depths[1].Story : depths[1])) != null ? stack1.StoryUrl : stack1), depth0))
    + "/"
    + escapeExpression(((helper = (helper = helpers.ChapterKey || (depth0 != null ? depth0.ChapterKey : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"ChapterKey","hash":{},"data":data}) : helper)))
    + "\" title=\""
    + escapeExpression(((helper = (helper = helpers.ChapterName || (depth0 != null ? depth0.ChapterName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"ChapterName","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.Idx || (depth0 != null ? depth0.Idx : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Idx","hash":{},"data":data}) : helper)))
    + "</a>\r\n";
},"4":function(depth0,helpers,partials,data,depths) {
  var stack1, helper, lambda=this.lambda, escapeExpression=this.escapeExpression, functionType="function", helperMissing=helpers.helperMissing;
  return "            <a class=\"rightwing\" href=\""
    + escapeExpression(lambda(((stack1 = (depths[1] != null ? depths[1].Story : depths[1])) != null ? stack1.StoryUrl : stack1), depth0))
    + "/"
    + escapeExpression(((helper = (helper = helpers.ChapterKey || (depth0 != null ? depth0.ChapterKey : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"ChapterKey","hash":{},"data":data}) : helper)))
    + "\" title=\""
    + escapeExpression(((helper = (helper = helpers.ChapterName || (depth0 != null ? depth0.ChapterName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"ChapterName","hash":{},"data":data}) : helper)))
    + "\">"
    + escapeExpression(((helper = (helper = helpers.Idx || (depth0 != null ? depth0.Idx : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Idx","hash":{},"data":data}) : helper)))
    + "</a>\r\n";
},"6":function(depth0,helpers,partials,data,depths) {
  var stack1, helper, lambda=this.lambda, escapeExpression=this.escapeExpression, functionType="function", helperMissing=helpers.helperMissing;
  return "                <a href=\""
    + escapeExpression(lambda(((stack1 = (depths[1] != null ? depths[1].Story : depths[1])) != null ? stack1.StoryUrl : stack1), depth0))
    + "/"
    + escapeExpression(((helper = (helper = helpers.ChapterKey || (depth0 != null ? depth0.ChapterKey : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"ChapterKey","hash":{},"data":data}) : helper)))
    + "\">\r\n                    <i class=\"icon icon-angle-left\" style=\"left: 10px;\"></i>\r\n                    <h2>"
    + escapeExpression(lambda(((stack1 = (depths[1] != null ? depths[1].Story : depths[1])) != null ? stack1.StoryName : stack1), depth0))
    + "</h2>\r\n                    <span>"
    + escapeExpression(((helper = (helper = helpers.ChapterName || (depth0 != null ? depth0.ChapterName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"ChapterName","hash":{},"data":data}) : helper)))
    + "</span>\r\n                </a>\r\n";
},"8":function(depth0,helpers,partials,data,depths) {
  var stack1, helper, lambda=this.lambda, escapeExpression=this.escapeExpression, functionType="function", helperMissing=helpers.helperMissing;
  return "                <a href=\""
    + escapeExpression(lambda(((stack1 = (depths[1] != null ? depths[1].Story : depths[1])) != null ? stack1.StoryUrl : stack1), depth0))
    + "/"
    + escapeExpression(((helper = (helper = helpers.ChapterKey || (depth0 != null ? depth0.ChapterKey : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"ChapterKey","hash":{},"data":data}) : helper)))
    + "\">\r\n                    <i class=\"icon icon-angle-right\" style=\"right: 10px;\"></i>\r\n                    <h2>"
    + escapeExpression(lambda(((stack1 = (depths[1] != null ? depths[1].Story : depths[1])) != null ? stack1.StoryName : stack1), depth0))
    + "</h2>\r\n                    <span>"
    + escapeExpression(((helper = (helper = helpers.ChapterName || (depth0 != null ? depth0.ChapterName : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"ChapterName","hash":{},"data":data}) : helper)))
    + "</span>\r\n                </a>\r\n";
},"10":function(depth0,helpers,partials,data) {
  return "    <div>\r\n        <p data-i18n=\"chapter-not-found\">Kapitel nicht gefunden</p>\r\n    </div>\r\n";
  },"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data,depths) {
  var stack1, buffer = "<div>\r\n";
  stack1 = helpers['if'].call(depth0, (depth0 != null ? depth0.CurrentChapter : depth0), {"name":"if","hash":{},"fn":this.program(1, data, depths),"inverse":this.program(10, data, depths),"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer + "</div>";
},"useData":true,"useDepths":true});

this["fsTemplates"]["tThumbGrid"] = Handlebars.template({"1":function(depth0,helpers,partials,data) {
  var helper, functionType="function", helperMissing=helpers.helperMissing, escapeExpression=this.escapeExpression;
  return "    <a href=\""
    + escapeExpression(((helper = (helper = helpers.Url || (depth0 != null ? depth0.Url : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Url","hash":{},"data":data}) : helper)))
    + "\">\r\n        <img src=\""
    + escapeExpression(((helper = (helper = helpers.Url200 || (depth0 != null ? depth0.Url200 : depth0)) != null ? helper : helperMissing),(typeof helper === functionType ? helper.call(depth0, {"name":"Url200","hash":{},"data":data}) : helper)))
    + "\" />\r\n    </a>    \r\n";
},"compiler":[6,">= 2.0.0-beta.1"],"main":function(depth0,helpers,partials,data) {
  var stack1, buffer = "<div class=\"thumbgrid\">\r\n";
  stack1 = helpers.each.call(depth0, (depth0 != null ? depth0.fotos : depth0), {"name":"each","hash":{},"fn":this.program(1, data),"inverse":this.noop,"data":data});
  if (stack1 != null) { buffer += stack1; }
  return buffer + "</div>";
},"useData":true});