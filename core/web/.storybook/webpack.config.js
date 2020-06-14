const path = require('path');

module.exports = ({ config, mode }) => {
    config.module.rules.push({
        test: /\.(ts|tsx)$/,
        loader: require.resolve('babel-loader'),
        options: {
            presets: [['react-app', { flow: false, typescript: true }]],
        },
    });

    config.resolve.extensions.push('.ts', '.tsx');

    config.module.rules.push({
        test: /\.less$/,
        loaders: [
            "style-loader",
            "css-loader",
            {
                loader: "less-loader",
                options: { javascriptEnabled: true }
            }
        ],
        include: path.resolve(__dirname, '../src/'),
    });


    console.log("config", config)
    return config;
};