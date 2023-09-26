-- Table: public.steam_followers
CREATE TABLE IF NOT EXISTS public.steam_followers
(
    datelocal date NOT NULL,
    game character varying COLLATE pg_catalog."default" NOT NULL,
    amount integer NOT NULL,
    CONSTRAINT followers_pkey PRIMARY KEY (game, datelocal)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.steam_followers
    OWNER to lumen;

REVOKE ALL ON TABLE public.steam_followers FROM grafana;

GRANT SELECT ON TABLE public.steam_followers TO grafana;

GRANT ALL ON TABLE public.steam_followers TO lumen;
