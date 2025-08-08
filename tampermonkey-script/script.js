// ==UserScript==
// @name         Wishlists & Sale LUMEN
// @namespace    http://elanis.eu/
// @version      2025-03-15
// @description  Sends the current session cookie to the Lumen API's SteamPartner Module, so it can query sales and wishlist data and save them in database for later reporting
// @author       Axel "Elanis" Soupé
// @match        https://partner.steampowered.com/*
// @icon         const LUMEN_SERVER_AUTH_HEADER_NAME = 'X-Api-Key';
// @grant        GM.cookie
// @grant        GM_xmlhttpRequest
// @homepageURL  https://github.com/L-U-M-E-N/Lumen.Modules.SteamPartner
// @connect      lumen.domain.tld
// ==/UserScript==

////////////////
// CONFIG
////////////////
const LUMEN_SERVER_URL = 'https://lumen.domain.tld/';
const LUMEN_SERVER_API_KEY = 'FILLME';

//// Constants
const LUMEN_SERVER_AUTH_HEADER_NAME = 'X-Api-Key';
const LUMEN_SERVER_ROUTE = 'SteamData/queryDataFromSteam';
const STEAM_PARTNERS_URL = 'https://partner.steampowered.com/';
const STEAM_PARTNERS_COOKIE_NAME = 'steamLoginSecure';

//// Execution
const cs = await GM.cookie.list({ url: STEAM_PARTNERS_URL, partitionKey: {} });
const cookie = cs.find((x) => x.name === STEAM_PARTNERS_COOKIE_NAME);
console.log(cookie);
GM.xmlHttpRequest({
  url: LUMEN_SERVER_URL + LUMEN_SERVER_ROUTE,
  method: "POST",
  data: JSON.stringify({ [STEAM_PARTNERS_COOKIE_NAME]: cookie.value }),
  headers: {
    "Content-Type": "application/json",
    [LUMEN_SERVER_AUTH_HEADER_NAME]: LUMEN_SERVER_API_KEY
  },
}).then((res) => {
    console.log(res);
    if(res.status === 202) {
        alert('Steam cookie has been submitted to LUMEN server and data has been updated.');
    }
}).catch(e => console.error(e));

