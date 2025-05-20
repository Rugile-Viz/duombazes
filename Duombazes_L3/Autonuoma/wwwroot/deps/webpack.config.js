const path = require('path');
const webpack = require('webpack')

module.exports = {
	entry: './src/js/index.js',
	mode: 'development',
	devtool : 'inline-source-map',
	
	output: {
		filename: 'main.js',
		path: path.resolve(__dirname, 'dist'),
	},

	module: {
		rules: [
			{
				test: /\.s[ac]ss$/i,
				use: [
					"style-loader",
					"css-loader",
					{
						loader: "sass-loader",
						options: {
							sassOptions: {
								//this will stop saas complaining about deprecated constructs in bootstrap
								quietDeps: true
							}
						}
					}
				],
			}
		]
	},

	//this is needed to bind various globals together inside modules
	plugins: [
        new webpack.ProvidePlugin({
            $: "jquery",
			jQuery: "jquery",
			Globalize : "globalize"
        })
	],
	
	//this is needed for clrdrjs to resolve correctly in globalize
	resolve: {
		alias: {
			'cldr$': 'cldrjs',
			'cldr': 'cldrjs/dist/cldr'
		}
	}
};