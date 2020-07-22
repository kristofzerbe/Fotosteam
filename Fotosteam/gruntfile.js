/// <binding BeforeBuild='build' Clean='build' />
/// <vs BeforeBuild='build' />
module.exports = function (grunt) {

    function randomString(max) {
        var text = "";
        var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        for (var i = 0; i < max; i++)
            text += possible.charAt(Math.floor(Math.random() * possible.length));

        return text;
    }

    var packageJson = grunt.file.readJSON('package.json');

    grunt.initConfig({
        pkg: packageJson,
        "file-creator": {
            "config": {
                "Scripts_Custom/fotosteam-config.js": function(fs, fd, done) {
                    fs.writeSync(fd,
                        'function fsConfig() {' + '\n' +
                        '    this.resourceSuffix = function() { ' + '\n' +
                        '        return "' + randomString(5) + '"; ' + '\n' +
                        '    };' + '\n' +
                        '    this.version = function() { ' + '\n' +
                        '        return "' + packageJson.version + '";' + '\n' +
                        '    };' + '\n' +
                        '}'
                    );
                    done();
                }
            }
        },
        //jshint: {
        //    files: [
        //        'gruntfile.js',
        //        'Scripts_Custom/fotosteam-*.js',
        //        'Scripts_Custom/Resource/fotosteam-*.js'
        //    ],
        //    options: {
        //        globals: {
        //            jQuery: true
        //        },
        //        loopfunc: true
        //    }
        //},
        jsonlint: {
            sample: {
                src: [
                    'Content/Locales/de.json',
                    'Content/Locales/en.json'
                ]
            }
        },
        handlebars: {
            compile: {
                options: {
                    namespace: "fsTemplates",
                    processName: function (filePath) {
                        return 't' + filePath.replace(/^Templates\//, '').replace(/\.html$/, '');
                    }
                },
                files: {
                    "Templates/compile/fotosteam-templates.js": ["Templates/**/*.html"]
                    //"Templates/build/templates-common.js": [
                    //    "Templates/error-template.html",
                    //    "Templates/sidebar-common-template.html",
                    //    "Templates/sidebar-member-template.html",
                    //    "Templates/logomenu-template.html",
                    //    "Templates/footer-template.html"
                    //]
                }
            }
        },
        concat: {
            basic: {
                options: {
                    separator: ';'
                },
                src: [
                    'Scripts_Custom/fotosteam-tools.js',
                    'Scripts_Custom/fotosteam-i18n.js',                    
                    'Scripts_Custom/fotosteam-objects.js',
                    'Scripts_Custom/fotosteam-authenticate.js',
                    'Scripts_Custom/fotosteam-route.js',
                    'Scripts_Custom/fotosteam-common.js',
                    'Scripts_Custom/fotosteam-member.js',
                    'Scripts_Custom/fotosteam-foto.js',
                    'Scripts_Custom/fotosteam-story.js',
                    'Scripts_Custom/fotosteam-stack.js',
                    'Scripts_Custom/fotosteam-overview.js',
                    'Templates/compile/fotosteam-templates.js',
                    'Scripts_Custom/fotosteam-config.js',
                    'Scripts_Custom/fotosteam-main.js'
                ],
                dest: 'Scripts/Build/fs<%= pkg.version %>/<%= pkg.name %>.js'
            },
            extras: {
                options: {
                    separator: ';'
                },
                src: [
                    'Scripts/jquery.cookie.js',
                    'Scripts/jquery.validate.js',
                    'Scripts/jquery.scrollTo.js',
                    'Scripts/jquery.mousewheel.js',
                    'Scripts/jquery.mCustomScrollbar.js',
                    'Scripts/moment-with-locales.js',
                    'Scripts/linq.js',
                    'Scripts/mousetrap.js',
                    'Scripts/handlebars-v2.0.js', //3.0, wenn https://github.com/gruntjs/grunt-contrib-handlebars/issues/136
                    'Scripts/jquery.toasty.js',
                    'Scripts/jquery.inputs.js',
                    'Scripts/jquery.dropit.js',
                    'Scripts/encoder.js',
                    'Scripts/jquery.tooltipster.js'
                ],
                dest: 'Scripts/Build/fs<%= pkg.version %>/extras.js',
                nonull: true,
            },
            styles: {
                src: [
                    'Content/CSS/app.css',
                    'Content/CSS/styles.css',
                    'Content/CSS/styles-mobile.css',
                    'Content/CSS/styles-medium.css',
                    'Content/CSS/styles-large.css',
                    'Content/CSS/styles-xlarge.css'
                ],
                dest: 'Content/Build/fs<%= pkg.version %>/<%= pkg.name %>.css'
            },
            specials: {
                src: [
                    'Content/CSS/normalize.css',
                    'Content/CSS/jquery.mCustomScrollbar.css',
                    'Content/CSS/owl.carousel.css',
                    'Content/CSS/owl.theme.css',
                    'Content/CSS/toasty.css',
                    'Content/CSS/inputs.css',
                    'Content/CSS/tooltipster.css'
                ],
                dest: 'Content/Build/fs<%= pkg.version %>/specials.css',
                nonull: true,
            }
        },
        uglify: {
            options: {
                banner: '/*! <%= pkg.name %> <%= grunt.template.today("dd-mm-yyyy") %> */\n',
                sourceMap: false
            },
            dist: {
                files: {
                    'Scripts/Build/fs<%= pkg.version %>/<%= pkg.name %>.min.js': ['<%= concat.basic.dest %>'],
                    'Scripts/Build/fs<%= pkg.version %>/fotosteam-dashboard.min.js': ['Scripts_Custom/Resource/fotosteam-dashboard.js'],
                    'Scripts/Build/fs<%= pkg.version %>/fotosteam-fotoedit.min.js': ['Scripts_Custom/Resource/fotosteam-fotoedit.js'],
                    'Scripts/Build/fs<%= pkg.version %>/fotosteam-multiedit.min.js': ['Scripts_Custom/Resource/fotosteam-multiedit.js'],
                    'Scripts/Build/fs<%= pkg.version %>/extras.min.js': ['<%= concat.extras.dest %>'],
                    'Scripts/Build/fs<%= pkg.version %>/jquery.tabulous.min.js': ['Scripts/Resource/jquery.tabulous.js']
                }
            },
        },
        cssmin: {
            dist: {
                files: {
                    'Content/Build/fs<%= pkg.version %>/<%= pkg.name %>.min.css': ['Content/Build/fs<%= pkg.version %>/<%= pkg.name %>.css'],
                    'Content/Build/fs<%= pkg.version %>/specials.min.css': ['Content/Build/fs<%= pkg.version %>/specials.css'],
                    'Content/Build/fs<%= pkg.version %>/tabulous.min.css': ['Content/CSS/Resource/tabulous.css']
                }
            }
        }
    });

    grunt.loadNpmTasks('grunt-file-creator');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-cssmin');
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-jshint');
    grunt.loadNpmTasks('grunt-contrib-handlebars');
    grunt.loadNpmTasks('grunt-jsonlint');

    //grunt.registerTask('build', ['file-creator', 'jshint', 'jsonlint', 'handlebars', 'concat', 'uglify', 'cssmin']);
    grunt.registerTask('build', ['file-creator', 'jsonlint', 'handlebars', 'concat', 'uglify', 'cssmin']);

};