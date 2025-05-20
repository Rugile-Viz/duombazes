import 'cldrjs'
import Globalize from 'globalize'

//load general stuff
import likelySubtags from 'cldr-core/supplemental/likelySubtags'
Globalize.load(likelySubtags);

import numberingSystems from 'cldr-core/supplemental/numberingSystems'
Globalize.load(numberingSystems);


//load en-US
import enNumbers from 'cldr-numbers-full/main/en/numbers'
Globalize.load(enNumbers);

import enCurrencies from 'cldr-numbers-full/main/en/currencies'
Globalize.load(enCurrencies);


//load lt
import ltNumbers from 'cldr-numbers-full/main/lt/numbers'
Globalize.load(ltNumbers);

import ltCurrencies from 'cldr-numbers-full/main/lt/currencies'
Globalize.load(ltCurrencies);


//expose parseNumber as parseFloat because jquery-validation-globalize needs parseFloat
Globalize.parseFloat = Globalize.parseNumber;


//set the globalize locale from the browser (this should correspond to system locale)
let knownLocales = ["en-US", "lt"];
let fallbackLocale = "en-US";
let locale = navigator.language;

if( !knownLocales.findIndex(kl => kl == locale) == -1 )
    locale = fallbackLocale

Globalize.locale(locale);