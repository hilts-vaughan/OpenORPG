module.exports = function(grunt) {

    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),

        dir: {
            deploy: {
                root: 'deploy/',
                js: 'deploy/js',
                assets: 'deploy/assets',
                atlas: 'deploy/assets/atlas',
                css: 'deploy/css'
            },
            src: {
                root: 'src/',
                lib: 'src/lib/',
                js: 'src/js/**/*.js',
                assets: 'src/assets/',
                css: 'src/css/',
                index: 'src/index.html'
            },
            assets: {
                root: 'assets/',
                atlas: 'assets/atlas/',
                maps: 'assets/maps/**/*.json',
                audio: 'assets/audio/'
            }
        },

        browserify: {
            basic: {
                src: ['src/js/start.js'],
                dest: 'deploy/js/GameDevTemplateJS.js',
                debug: true
            },

            options: {
                debug: true             
            }


        },

        mkdir: {
            all: {
                options: {
                    mode: 0700,
                    create: [
                        'assets/atlas', 'assets/maps', 'assets/audio',
                        'deploy',
                        'resources',
                        'src/css', 'src/js', 'src/lib',
                        'tasks'
                    ]
                },
            },
        },

        clean: ['<%= dir.deploy.root %>'],

        copy: {

            devcontent: {
                cwd: '../../DevContent/',
                src: ['**'],
                dest: '<%= dir.deploy.assets %>',
                expand: true
            },

            index: {
                cwd: '<%= dir.src.root %>',
                src: ['index.html'],
                dest: '<%= dir.deploy.root %>',
                expand: true
            },


            lib: {
                files: [{
                    cwd: '<%= dir.src.lib %>',
                    src: ['**'],
                    dest: '<%= dir.deploy.js %>',
                    expand: true
                }]
            },
            assets: {
                files: [{
                        cwd: '<%= dir.assets.root %>',
                        src: ['audio/**/*.*'],
                        dest: '<%= dir.deploy.assets %>',
                        expand: true
                    }, {
                        cwd: '<%= dir.assets.root %>',
                        src: ['maps/**/*.json'],
                        dest: '<%= dir.deploy.assets %>',
                        expand: true
                    },

                    {
                        cwd: '<%= dir.assets.root %>',
                        src: ['ui/**/*.*'],
                        dest: '<%= dir.deploy.assets %>',
                        expand: true
                    }

                ]
            },
            css: {
                files: [{
                    cwd: '<%= dir.src.root %>',
                    src: ['css/**/*.*'],
                    dest: '<%= dir.deploy.root %>',
                    expand: true
                }]
            }
        },

        replace: {
            index: {
                options: {
                    patterns: [{
                        match: 'GameNamePretty',
                        replacement: '<%= pkg.namePretty %>'
                    }, {
                        match: 'GameName',
                        replacement: '<%= pkg.name %>'
                    }]
                },
                files: [{
                    expand: true,
                    flatten: true,
                    src: ['<%= dir.src.index %>'],
                    dest: '<%= dir.deploy.root %>'
                }]
            }
        },

        concat: {
            game: {
                options: {
                    process: {
                        data: {
                            version: '<%= pkg.version %>',
                            buildDate: '<%= grunt.template.today() %>'
                        }
                    }
                },
                src: ['<%= dir.src.js %>'],
                dest: '<%= dir.deploy.js %>/<%= pkg.name %>.js'
            }
        },

        uglify: {
            game: {
                options: {
                    banner: '/*! GameDevTemplateJS <%= pkg.version %> | ' +
                        '(c) 2013 Thomas Viktil */ \n'
                },
                src: ['<%= concat.game.dest %>'],
                dest: '<%= dir.deploy.js %>/<%= pkg.name %>.min.js'
            }
        },

        texturepacker: {
            misc: {
                targetdir: '<%= dir.deploy.atlas %>',
                dirs: [
                    '<%= dir.assets.atlas %>boxes',
                    '<%= dir.assets.atlas %>hud'
                ]
            }
        },

        watch: {
            source: {
                files: '<%= dir.src.js %>',
                tasks: ['updatejs'],
                options: {
                    livereload: true
                }
            },
            maps: {
                files: '<%= dir.assets.maps %>',
                tasks: ['copy:assets'],
                options: {
                    livereload: true
                }
            },
            atlas: {
                files: '<%= dir.assets.atlas %>**/*.*',
                tasks: ['texturepacker'],
                options: {
                    livereload: true
                }
            },
            index: {
                files: '<%= dir.src.index %>',
                tasks: ['replace'],
                options: {
                    livereload: true
                }
            }
        },

        connect: {
            root: {
                options: {
                    port: 2020,
                    base: './deploy',
                    livereload: true
                }
            }
        },

        open: {
            dev: {
                path: 'http://localhost:2020/index.html',
                app: 'Google Chrome'
            }
        }
    });

    grunt.loadNpmTasks('grunt-contrib-clean');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-connect');
    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-open');
    grunt.loadNpmTasks('grunt-exec');
    grunt.loadNpmTasks('grunt-replace');
    grunt.loadNpmTasks('grunt-mkdir');
    grunt.loadNpmTasks('grunt-browserify');


    grunt.loadTasks('./tasks');

    grunt.registerTask('build', [
        'copy',
        'browserify'
    ]);
    grunt.registerTask('updatejs', [
        'browserify'
    ]);
    grunt.registerTask('default', [
        'clean',
        'build',
        'connect',
        'watch'
    ]);

    grunt.registerTask('init', ['mkdir']);
}