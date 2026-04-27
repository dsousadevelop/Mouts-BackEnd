--
-- PostgreSQL database dump
--

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
-- SET transaction_timeout = 0; -- Removido: incompatível com Postgres 14

SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

-- Removido ALTER SCHEMA public OWNER TO developer para evitar erro de permissão no Docker
-- O usuário definido em POSTGRES_USER já será o dono do banco.


SET default_tablespace = '';
SET default_table_access_method = heap;

-- Sequências
CREATE SEQUENCE IF NOT EXISTS public.address_id_seq START WITH 1 INCREMENT BY 1 NO MINVALUE NO MAXVALUE CACHE 1;
CREATE SEQUENCE IF NOT EXISTS public.cartitems_id_seq START WITH 1 INCREMENT BY 1 NO MINVALUE NO MAXVALUE CACHE 1;
CREATE SEQUENCE IF NOT EXISTS public.carts_id_seq START WITH 1 INCREMENT BY 1 NO MINVALUE NO MAXVALUE CACHE 1;
CREATE SEQUENCE IF NOT EXISTS public.category_id_seq START WITH 1 INCREMENT BY 1 NO MINVALUE NO MAXVALUE CACHE 1;
CREATE SEQUENCE IF NOT EXISTS public.products_id_seq START WITH 1 INCREMENT BY 1 NO MINVALUE NO MAXVALUE CACHE 1;
CREATE SEQUENCE IF NOT EXISTS public.users_id_seq START WITH 1 INCREMENT BY 1 NO MINVALUE NO MAXVALUE CACHE 1;

-- Tabelas
CREATE TABLE public.address (
    id integer NOT NULL,
    user_id integer NOT NULL,
    city character varying(30) NOT NULL,
    street character varying(30) NOT NULL,
    number smallint NOT NULL,
    zipcode character varying(8) NOT NULL,
    geolocation_lat character varying(15),
    geolocation_long character varying(15),
    createdat timestamp with time zone NOT NULL,
    updatedat timestamp with time zone
);

CREATE TABLE public.cartitems (
    id integer NOT NULL,
    cart_id integer NOT NULL,
    product_id integer NOT NULL,
    quantity numeric(10,2),
    createdat timestamp without time zone NOT NULL,
    updatedat timestamp without time zone,
    discount numeric(10,2),
    subtotal numeric(12,2) NOT NULL,
    total numeric(12,2) NOT NULL,
    unit_price numeric(10,2) NOT NULL
);

CREATE TABLE public.carts (
    id integer NOT NULL,
    user_id integer NOT NULL,
    createdat timestamp with time zone NOT NULL,
    updatedat timestamp with time zone,
    total_amount numeric(18,2) NOT NULL,
    is_cancelled boolean NOT NULL,
    date timestamp with time zone DEFAULT now() NOT NULL
);

CREATE TABLE public.category (
    id integer NOT NULL,
    description character varying(40) NOT NULL,
    createdat timestamp with time zone NOT NULL,
    updatedat timestamp with time zone
);

CREATE TABLE public.products (
    id integer NOT NULL,
    title character varying(60) NOT NULL,
    price numeric(10,2) NOT NULL,
    description character varying(100) NOT NULL,
    category_id integer NOT NULL,
    image character varying(200),
    rating_rate numeric,
    rating_count smallint,
    createdat timestamp with time zone NOT NULL,
    updatedat timestamp with time zone
);

CREATE TABLE public.users (
    id integer NOT NULL,
    username character varying(100) NOT NULL,
    firstname character varying(40) NOT NULL,
    lastname character varying(40) NOT NULL,
    phone character varying(11) NOT NULL,
    status smallint NOT NULL,
    roles smallint NOT NULL,
    email character varying(100) NOT NULL,
    password character varying(100) NOT NULL,
    createdat timestamp with time zone NOT NULL,
    updatedat timestamp with time zone
);

-- Sequências e Defaults
ALTER TABLE ONLY public.address ALTER COLUMN id SET DEFAULT nextval('public.address_id_seq'::regclass);
ALTER TABLE ONLY public.cartitems ALTER COLUMN id SET DEFAULT nextval('public.cartitems_id_seq'::regclass);
ALTER TABLE ONLY public.carts ALTER COLUMN id SET DEFAULT nextval('public.carts_id_seq'::regclass);
ALTER TABLE ONLY public.category ALTER COLUMN id SET DEFAULT nextval('public.category_id_seq'::regclass);
ALTER TABLE ONLY public.products ALTER COLUMN id SET DEFAULT nextval('public.products_id_seq'::regclass);
ALTER TABLE ONLY public.users ALTER COLUMN id SET DEFAULT nextval('public.users_id_seq'::regclass);


-- Constraints e PKs
ALTER TABLE ONLY public.users ADD CONSTRAINT users_pk PRIMARY KEY (id);
ALTER TABLE ONLY public.address ADD CONSTRAINT address_pk PRIMARY KEY (id);
ALTER TABLE ONLY public.cartitems ADD CONSTRAINT cartitems_pk PRIMARY KEY (id);
ALTER TABLE ONLY public.carts ADD CONSTRAINT carts_pk PRIMARY KEY (id);
ALTER TABLE ONLY public.category ADD CONSTRAINT category_pk PRIMARY KEY (id);
ALTER TABLE ONLY public.products ADD CONSTRAINT products_pk PRIMARY KEY (id);

-- FKs
ALTER TABLE ONLY public.address ADD CONSTRAINT address_users_fk FOREIGN KEY (user_id) REFERENCES public.users(id);
ALTER TABLE ONLY public.cartitems ADD CONSTRAINT cartitems_products_fk FOREIGN KEY (product_id) REFERENCES public.products(id);
ALTER TABLE ONLY public.carts ADD CONSTRAINT carts_users_fk FOREIGN KEY (user_id) REFERENCES public.users(id);
ALTER TABLE ONLY public.products ADD CONSTRAINT products_category_fk FOREIGN KEY (category_id) REFERENCES public.category(id);