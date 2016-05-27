/*
This file in the main entry point for defining grunt tasks and using grunt plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkID=513275&clcid=0x409
*/
module.exports = function (grunt) {
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-watch');

    grunt.initConfig({
        concat: {
            all: {
                src: ['Scripts/app.js', 'Scripts/**/*.js'],
                dest: 'wwwroot/app.js'
            }
        },
        
        watch: {
            scripts: {
                files: ['Scripts/**/*.js'],
                task: ['concat']
            }
        }
    });

    grunt.registerTask('default', ['concat', 'watch']);
};