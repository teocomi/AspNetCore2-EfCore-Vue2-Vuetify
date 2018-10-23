const path = require('path');
const webpack = require('webpack');
const ExtractTextPlugin = require('extract-text-webpack-plugin');
const bundleOutputDir = './wwwroot/dist';
var OptimizeCssAssetsPlugin = require('optimize-css-assets-webpack-plugin');
const safe = require('postcss-safe-parser');

module.exports = (env) => {
    const isDevBuild = !(env && env.prod);
    return [{
        mode: isDevBuild ? 'development' : 'production',
        stats: { modules: false },
        entry: {
            'main': ['babel-polyfill', './ClientApp/boot-app.js'] },
        resolve: {
            extensions: ['.js', '.vue'],
            alias: {
                'vue$': 'vue/dist/vue',
                'components': path.resolve(__dirname, './ClientApp/components'),
                'views': path.resolve(__dirname, './ClientApp/views'),
                'utils': path.resolve(__dirname, './ClientApp/utils'),
                'api': path.resolve(__dirname, './ClientApp/store/api')
            }
        },
        output: {
            path: path.join(__dirname, bundleOutputDir),
            filename: '[name].js',
            publicPath: '/dist/'
        },
        module: {
            rules: [
                {
                    test: /\.vue$/,
                    use: 'vue-loader',
                    include: /ClientApp/
                },
                {
                    test: /\.js$/,
                    include: [ // use `include` vs `exclude` to white-list vs black-list
                        /ClientApp/, // white-list your app source files
                    ], use: 'babel-loader' },
                { test: /\.css$/, use: ['style-loader', 'css-loader'] },
               // { test: /\.(jpe?g|png|gif|svg|eot|woff|ttf|svg|woff2)$/, use: "file?name=[name].[ext]" }
                { test: /\.(jpe?g|png|gif|svg|eot|woff|ttf|svg|woff2)$/, use: 'url-loader?limit=25000' },
            ]
        },
        optimization: {
            minimizer: [
                new OptimizeCssAssetsPlugin({
                    cssProcessorOptions: {
                        parser: safe,
                        discardComments: {
                            removeAll: true
                        }
                    }
                })
            ].concat(isDevBuild ? [
                // Plugins that apply in development builds only
            ] : [
                    // Plugins that apply in production builds only
                    //new webpack.optimize.UglifyJsPlugin(),
                    new ExtractTextPlugin('site.css')
                ]),
        },
        plugins: [
            //new webpack.DllReferencePlugin({
            //    context: __dirname,
            //    manifest: require('./wwwroot/dist/vendor-manifest.json')
            //}),

        ].concat(isDevBuild ? [
            // Plugins that apply in development builds only
            new webpack.SourceMapDevToolPlugin({
                filename: '[file].map', // Remove this line if you prefer inline source maps
                moduleFilenameTemplate: path.relative(bundleOutputDir, '[resourcePath]') // Point sourcemap entries to the original file locations on disk
            })
        ] : [
                // Plugins that apply in production builds only
            ])
    }];
};
