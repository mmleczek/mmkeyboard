fx_version 'cerulean'
game 'gta5'

author 'mmleczek (mbinary.pl)'
version 'v0.0.1'

ui_page 'ui/ui.html'

files {
    'ui/*.css',
    'ui/*.js',
    'ui/*.html'
}

client_script 'client.net.dll'

exports {
    'Show',
    'IsVisible',
    'Hide'
}