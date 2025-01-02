require.config({
    shim: {
        'fullcalendar': ['moment', 'jquery'],
    },
    paths: {
        'fullcalendar': rootPath + 'assets/plugins/fullcalendar/js/fullcalendar.min',
        'moment': rootPath + 'assets/plugins/fullcalendar/js/moment.min',
    }
});