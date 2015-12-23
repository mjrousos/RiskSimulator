/// <binding Clean='clean' />
"use strict";

var gulp = require("gulp"),
  rimraf = require("rimraf"),
  concat = require("gulp-concat"),
  cssmin = require("gulp-cssmin"),
  uglify = require("gulp-uglify"),
  project = require("./project.json");

var paths = {
  assets: "./assets/",
  webroot: "./wwwroot/"
};

paths.js = paths.assets + "scripts/**/*.js";
paths.minJs = paths.assets + "scripts/**/*.min.js";
paths.css = paths.assets + "styles/**/*.css";
paths.minCss = paths.assets + "styles/**/*.min.css";
paths.images = paths.assets + "images/**/*.*";

paths.concatJsDest = paths.webroot + "scripts/site.min.js";
paths.concatCssDest = paths.webroot + "styles/site.min.css";
paths.copyJsDest = paths.webroot + "scripts/site.js";
paths.copyCssDest = paths.webroot + "styles/site.css";
paths.imagesDest = paths.webroot + "images";

gulp.task("clean:js", function(cb) {
  rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function(cb) {
  rimraf(paths.concatCssDest, cb);
});

gulp.task("clean", ["clean:js", "clean:css"]);

gulp.task("min:js", function() {
  gulp.src([paths.js, "!" + paths.minJs], {
      base: "."
    })
    .pipe(concat(paths.concatJsDest))
    .pipe(uglify())
    .pipe(gulp.dest("."));
});

gulp.task("min:css", function() {
     gulp.src([paths.css, "!" + paths.minCss])
     .pipe(concat(paths.concatCssDest))
     .pipe(cssmin())
     .pipe(gulp.dest("."));
});

gulp.task("copy:js", function() {
  gulp.src([paths.js, "!" + paths.minJs], {
      base: "."
    })
    .pipe(concat(paths.copyJsDest))
    .pipe(gulp.dest("."));
});

gulp.task("copy:css", function() {
  gulp.src([paths.css, "!" + paths.minCss])
    .pipe(concat(paths.copyCssDest))
    .pipe(gulp.dest("."));
});

gulp.task("copy:images", function() {
  gulp.src([paths.images])
    .pipe(gulp.dest(paths.imagesDest));
});

gulp.task("min", ["min:js", "min:css"]);
gulp.task("copy", ["copy:js", "copy:css", "copy:images"]);
gulp.task("deploy", ["min","copy"]);
