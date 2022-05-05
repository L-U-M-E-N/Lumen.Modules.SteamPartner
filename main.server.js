import { QueryPackageSalesForCSV, QueryWishlistActionsForCSV } from './SteamCSV.js';

const COOKIE_FORMAT = (runAs) => config.COOKIE_FORMAT.replace('${runAs}', runAs);

export default class SteamSoldWishlist {
	static running = false;

	static init() {
		SteamSoldWishlist.update();

		clearInterval(SteamSoldWishlist.interval);
		SteamSoldWishlist.interval = setInterval(SteamSoldWishlist.update, 3 * 60 * 60 * 1000); // Update every 3 hours
	}

	static close() {
		clearInterval(SteamSoldWishlist.interval);
	}

	static async update() {
		if(SteamSoldWishlist.running) {
			return;
		}
		SteamSoldWishlist.running = true;

		log('Updating Steam status ...', 'info');

		try {
			await SteamSoldWishlist.UpdateSoldAmount();
		} catch(e) {
			log(' ' + e.message, 'error');
			SteamSoldWishlist.running = false;
			return;
		}
		try {
			await SteamSoldWishlist.UpdateWishlistAmount();
		} catch(e) {
			log(' ' + e.message, 'error');
			SteamSoldWishlist.running = false;
			return;
		}
		SteamSoldWishlist.running = false;
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

		for(const currentPackage of config.soldPackages) {
			const data = await QueryPackageSalesForCSV({ ...currentPackage, cookie: COOKIE_FORMAT(currentPackage.runAs) });

			for(const line of data) {
				const currentDate = (new Date(line['Date'] + 'T00:00:00.000Z')).getTime();

				if(dbData.find((data) =>
					data.bundle_id == line['Bundle(ID#)'] &&
					data.product_id == line['Product(ID#)'] &&
					(data.date.getTime() == currentDate) &&
					data.country_code == line['Country Code'] &&
					data.type == line["Type"] &&
					data.gross_units_sold == line["Gross Units Sold"] &&
					data.net_units_sold == line["Net Units Sold"] &&
					data.gross_steam_sale_usd == line["Gross Steam Sales (USD)"] &&
					data.net_steam_sale_usd == line["Net Steam Sales (USD)"]
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

		for(const currentPackage of config.wishlistApps) {
			const data = await QueryWishlistActionsForCSV({ ...currentPackage, cookie: COOKIE_FORMAT(currentPackage.runAs) });

			for(const line of data) {
				const currentDate = (new Date(line['DateLocal'] + 'T00:00:00.000Z')).getTime();

				if(dbData.find((data) =>
					data.datelocal.getTime() == currentDate &&
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