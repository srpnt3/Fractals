const gulp = require('gulp');
const postcss = require('gulp-postcss');
const autoprefixer = require('autoprefixer');
const cssnano = require('cssnano');
const rename = require('gulp-rename');
const responsive = require('gulp-responsive');
const minify = require('gulp-minify');

gulp.task('css', () => {
	return gulp.src('src/css/*.css')
		.pipe(postcss([
			autoprefixer(),
			cssnano
		]))
		.pipe(gulp.dest('../docs/css/'));
});

gulp.task('js', () => {
	return gulp.src('src/scripts/*.js')
		.pipe(minify({
			noSource: true,
			ext: {
				min: '.js'
			}
		}))
		.pipe(gulp.dest('../docs/scripts/'));
});

gulp.task('media', () => {
	return gulp.src('src/media/**/*')
		.pipe(gulp.dest('../docs/media/'));
});

gulp.task('html', () => {
	return gulp.src('src/*.html')
		.pipe(gulp.dest('../docs/'));
});

gulp.task('gallery-images', () => {
	let i = 0;
	return gulp.src('src/media/gallery/images/*')
		.pipe(responsive({'*': {width: 1920}}))
		.pipe(rename(path => path.basename = '' + i++))
		.pipe(gulp.dest('src/media/gallery/images/'));
});

gulp.task('gallery-videos', () => {
	let i = 0;
	return gulp.src('src/media/gallery/videos/*')
		.pipe(rename(path => path.basename = '' + i++))
		.pipe(gulp.dest('src/media/gallery/videos/'));
});

gulp.task('gallery', gulp.parallel('gallery-images', 'gallery-videos'));

gulp.task('default', gulp.parallel('css', 'js', 'media', 'html'));