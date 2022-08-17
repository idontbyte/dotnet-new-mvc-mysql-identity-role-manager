const gulp = require('gulp');
const { src, dest } = require('gulp');
const sass = require('gulp-sass')(require('sass'));
const concat = require('gulp-concat');
const uglify = require('gulp-uglify');
const { watch } = require('gulp');

exports.sass = function () {
  return src('scss/**/*.scss')
    .pipe(sass())
    .pipe(dest('../wwwroot/css/'));
}

exports.js = function () {
  return src(['node_modules/jquery/dist/jquery.js',
  'node_modules/bootstrap/dist/js/bootstrap.bundle.js',
  'js/**/*.js'])
    .pipe(concat('main.js'))
    .pipe(uglify())
    .pipe(dest('../wwwroot/js/'));
}

exports.watch = function (cb) {
  watch(['scss/**/*.scss'], gulp.series('sass'))
  watch(['js/**/*.js'], gulp.series('js'))
  cb();
};

exports.default = gulp.parallel(exports.sass, exports.js, exports.watch)