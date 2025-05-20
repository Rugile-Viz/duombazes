//pull in jQuery and plugins
import $ from 'jquery'

//pull in globalize-js stuff
import './configureGlobalize.js'

//pull jquery validation
import 'jquery-validation'
import 'jquery-validation-globalize'
import 'jquery-validation-unobtrusive'

//pull in jquery datetime picker
import 'jquery-datetimepicker'

//pull in datatables
import 'datatables.net'

//pull in bootstrap scripts
import 'bootstrap'

//pull in style sheets
import '../css/index.scss'

//expose jQuery outside the module built by webpack
window.__jQuery = $;

//transfer SQL queries passed in page header script to the browser console
if( base64JsonSqlQueries != null ) {
    //decode into byte array manually because of chrome based browsers
    let binStrJsonSqlQueries = atob(base64JsonSqlQueries)
    let byteArrJsonSqlQueries = new Uint8Array(binStrJsonSqlQueries.length)
    byteArrJsonSqlQueries = byteArrJsonSqlQueries.map((it, idx) => binStrJsonSqlQueries.charCodeAt(idx))

    //decode into json
    let jsonSqlQueries = (new TextDecoder("utf-8")).decode(byteArrJsonSqlQueries);
    let sqlQueries = JSON.parse(jsonSqlQueries);

    //decode success? output contents to browser console
    if( sqlQueries instanceof Array ) {
        sqlQueries.forEach(sqlq => console.info(sqlq));
    }
}