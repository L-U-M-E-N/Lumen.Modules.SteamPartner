import { QueryPackageSalesForCSV, QueryWishlistActionsForCSV } from './SteamCSV.js';

const soldPackages = [
	{ id: 000000, name: 'APP NAME', runAs: 000000 },
];

const wishlistApps = [
	{ id: 000000, name: 'APP NAME', runAs: 000000 },
];

const COOKIE_FORMAT = (runAs) =>
	`steamLoginSecure=; steamMachineAuth; steamworksRunas=${runAs};`;

export default class SteamSoldWishlist {
	static init() {
		SteamSoldWishlist.update();

		clearInterval(SteamSoldWishlist.interval);
		SteamSoldWishlist.interval = setInterval(SteamSoldWishlist.update, 3 * 60 * 60 * 1000); // Update every 3 hours
	}

	static close() {
		clearInterval(SteamSoldWishlist.interval);
	}

	static async update() {
		log('Updating Steam status ...', 'info');

		try {
			await SteamSoldWishlist.UpdateSoldAmount();
		} catch(e) {
			log(' ' + e.message, 'error');
			return;
		}
		try {
			await SteamSoldWishlist.UpdateWishlistAmount();
		} catch(e) {
			log(' ' + e.message, 'error');
			return;
		}
	}

	static async UpdateSoldAmount() {
		const query = `
			INSERT INTO public.steam_sold(
				date, bundle_id, bundle_name, product_id, product_name, type, game, plateform, country_code, country,
				region, gross_units_sold, chargebacks_returns, net_units_sold, base_price, sale_price, currency,
				gross_steam_sale_usd, chargebacks_returns_usd, vat_usd, net_steam_sale_usd, tag
			)
			VALUES (
				$1, $2, $3, $4, $5, $6, $7, $8, $9, $10,
				$11, $12, $13, $14, $15, $16, $17,
				$18, $19, $20, $21, $22
			)
		`;

		const dbData = (await Database.execQuery('SELECT * FROM steam_sold')).rows;

		for(const currentPackage of soldPackages) {
			const data = await QueryPackageSalesForCSV({ ...currentPackage, cookie: COOKIE_FORMAT(currentPackage.runAs) });

			for(const line of data) {
				const currentDate = (new Date(line['Date'])).getTime() - 2 * 60 * 60 * 1000;
				const currentDate2 = (new Date(line['Date'])).getTime() - 60 * 60 * 1000;

				if(dbData.find((data) =>
					data.bundle_id == line['Bundle(ID#)'] &&
					data.product_id == line['Product(ID#)'] &&
					(data.date.getTime() == currentDate || data.date.getTime() == currentDate2) &&
					data.country_code == line['Country Code']
				)) {
					continue;
				}

				try {
					await Database.execQuery(
						query,
						Object.values(line)
					);
				} catch(e) {
					// Do nothing
				}
			}
		}

		log('Saved Steam sold status', 'info');
	}

	static async UpdateWishlistAmount() {
		const query = `
			INSERT INTO public.steam_wishlists(datelocal, game, adds, deletes, purchases_and_activations, gifts)
			VALUES ($1, $2, $3, $4, $5, $6)
		`;

		const dbData = (await Database.execQuery('SELECT * FROM steam_wishlists')).rows;

		for(const currentPackage of wishlistApps) {
			const data = await QueryWishlistActionsForCSV({ ...currentPackage, cookie: COOKIE_FORMAT(currentPackage.runAs) });

			for(const line of data) {
				const currentDate = (new Date(line['DateLocal'])).getTime() - 2 * 60 * 60 * 1000;
				const currentDate2 = (new Date(line['DateLocal'])).getTime() - 60 * 60 * 1000;

				if(dbData.find((data) =>
					(data.datelocal.getTime() == currentDate || data.datelocal.getTime() == currentDate2) &&
					data.game == line['Game']
				)) {
					continue;
				}

				try {
					await Database.execQuery(
						query,
						Object.values(line)
					);
				} catch(e) {
					// Do nothing
				}
			}
		}

		log('Saved Steam wishlist status', 'info');
	}
}

SteamSoldWishlist.init();