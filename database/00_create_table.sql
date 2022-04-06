-- Table: public.steam_sold

-- DROP TABLE IF EXISTS public.steam_sold;

CREATE TABLE IF NOT EXISTS public.steam_sold
(
    date date NOT NULL,
    bundle_id integer NOT NULL,
    bundle_name character varying COLLATE pg_catalog."default" NOT NULL,
    product_id integer NOT NULL,
    product_name character varying COLLATE pg_catalog."default" NOT NULL,
    type character varying COLLATE pg_catalog."default" NOT NULL,
    game character varying COLLATE pg_catalog."default" NOT NULL,
    plateform character varying COLLATE pg_catalog."default" NOT NULL,
    country_code character varying COLLATE pg_catalog."default" NOT NULL,
    country character varying COLLATE pg_catalog."default" NOT NULL,
    region character varying COLLATE pg_catalog."default" NOT NULL,
    gross_units_sold integer NOT NULL,
    chargebacks_returns integer NOT NULL,
    net_units_sold integer NOT NULL,
    base_price double precision NOT NULL,
    sale_price double precision NOT NULL,
    currency character varying COLLATE pg_catalog."default" NOT NULL,
    gross_steam_sale_usd double precision NOT NULL,
    chargebacks_returns_usd double precision NOT NULL,
    vat_usd double precision NOT NULL,
    net_steam_sale_usd double precision NOT NULL,
    tag character varying COLLATE pg_catalog."default" NOT NULL
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.steam_sold
    OWNER to lumen;

-- Table: public.steam_wishlists

-- DROP TABLE IF EXISTS public.steam_wishlists;

CREATE TABLE IF NOT EXISTS public.steam_wishlists
(
    datelocal date NOT NULL,
    game character varying COLLATE pg_catalog."default" NOT NULL,
    adds integer NOT NULL,
    deletes integer NOT NULL,
    purchases_and_activations integer NOT NULL,
    gifts integer NOT NULL,
    CONSTRAINT wishlists_pkey PRIMARY KEY (game, datelocal)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.steam_wishlists
    OWNER to lumen;