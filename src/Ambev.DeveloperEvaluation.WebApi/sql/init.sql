--
-- PostgreSQL database dump
--

-- Dumped from database version 14.22 (Debian 14.22-1.pgdg14+1)
-- Dumped by pg_dump version 14.22 (Debian 14.22-1.pgdg14+1)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: developer_evaluation; Type: DATABASE; Schema: -; Owner: developer
--

-- CREATE DATABASE developer_evaluation WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE = 'en_US.utf8';

-- cria usuário (se não existir)
-- DO $$
-- BEGIN
--    IF NOT EXISTS (SELECT FROM pg_roles WHERE rolname = 'developer') THEN
--       CREATE ROLE developer LOGIN PASSWORD 'v83ay74';
--    END IF;
-- END
-- $$;

-- -- conecta no banco
-- \c developer_evaluation;

-- ALTER DATABASE developer_evaluation OWNER TO developer;

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: address; Type: TABLE; Schema: public; Owner: developer
--

CREATE TABLE public.address (
    id integer NOT NULL,
    user_id integer NOT NULL,
    city character varying(30) NOT NULL,
    street character varying(30) NOT NULL,
    number smallint NOT NULL,
    zipcode character varying(8) NOT NULL,
    geolocation_lat character varying(15),
    geolocation_long character varying(15),
    createdat timestamp without time zone NOT NULL,
    updatedat timestamp without time zone
);


ALTER TABLE public.address OWNER TO developer;

--
-- Name: address_id_seq; Type: SEQUENCE; Schema: public; Owner: developer
--

CREATE SEQUENCE public.address_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.address_id_seq OWNER TO developer;

--
-- Name: address_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: developer
--

ALTER SEQUENCE public.address_id_seq OWNED BY public.address.id;


--
-- Name: cartitems; Type: TABLE; Schema: public; Owner: developer
--

CREATE TABLE public.cartitems (
    id integer NOT NULL,
    cart_id integer NOT NULL,
    product_id integer NOT NULL,
    quantity numeric(10,2),
    createdat timestamp without time zone NOT NULL,
    updatedat timestamp without time zone,
    discount numeric(10,2),
    subtotal numeric(12,2) NOT NULL,
    total numeric(12,2) NOT NULL
);


ALTER TABLE public.cartitems OWNER TO developer;

--
-- Name: cartitems_id_seq; Type: SEQUENCE; Schema: public; Owner: developer
--

CREATE SEQUENCE public.cartitems_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.cartitems_id_seq OWNER TO developer;

--
-- Name: cartitems_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: developer
--

ALTER SEQUENCE public.cartitems_id_seq OWNED BY public.cartitems.id;


--
-- Name: carts; Type: TABLE; Schema: public; Owner: developer
--

CREATE TABLE public.carts (
    id integer NOT NULL,
    user_id integer NOT NULL,
    date time without time zone NOT NULL,
    createdat timestamp without time zone NOT NULL,
    updatedat timestamp without time zone
);


ALTER TABLE public.carts OWNER TO developer;

--
-- Name: carts_id_seq; Type: SEQUENCE; Schema: public; Owner: developer
--

CREATE SEQUENCE public.carts_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.carts_id_seq OWNER TO developer;

--
-- Name: carts_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: developer
--

ALTER SEQUENCE public.carts_id_seq OWNED BY public.carts.id;


--
-- Name: category; Type: TABLE; Schema: public; Owner: developer
--

CREATE TABLE public.category (
    id integer NOT NULL,
    description character varying(40) NOT NULL,
    createdat timestamp without time zone NOT NULL,
    updatedat timestamp without time zone
);


ALTER TABLE public.category OWNER TO developer;

--
-- Name: category_id_seq; Type: SEQUENCE; Schema: public; Owner: developer
--

CREATE SEQUENCE public.category_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.category_id_seq OWNER TO developer;

--
-- Name: category_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: developer
--

ALTER SEQUENCE public.category_id_seq OWNED BY public.category.id;


--
-- Name: products; Type: TABLE; Schema: public; Owner: developer
--

CREATE TABLE public.products (
    id integer NOT NULL,
    title character varying(60) NOT NULL,
    price numeric(10,2) NOT NULL,
    description character varying(100) NOT NULL,
    category_id integer NOT NULL,
    image character varying(200),
    rating_rate numeric,
    rating_count smallint,
    createdat timestamp without time zone NOT NULL,
    updatedat timestamp without time zone
);


ALTER TABLE public.products OWNER TO developer;

--
-- Name: products_id_seq; Type: SEQUENCE; Schema: public; Owner: developer
--

CREATE SEQUENCE public.products_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.products_id_seq OWNER TO developer;

--
-- Name: products_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: developer
--

ALTER SEQUENCE public.products_id_seq OWNED BY public.products.id;


--
-- Name: users; Type: TABLE; Schema: public; Owner: developer
--

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
    createdat timestamp without time zone NOT NULL,
    updatedat timestamp without time zone
);


ALTER TABLE public.users OWNER TO developer;

--
-- Name: users_id_seq; Type: SEQUENCE; Schema: public; Owner: developer
--

CREATE SEQUENCE public.users_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.users_id_seq OWNER TO developer;

--
-- Name: users_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: developer
--

ALTER SEQUENCE public.users_id_seq OWNED BY public.users.id;


--
-- Name: address id; Type: DEFAULT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public.address ALTER COLUMN id SET DEFAULT nextval('public.address_id_seq'::regclass);


--
-- Name: cartitems id; Type: DEFAULT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public.cartitems ALTER COLUMN id SET DEFAULT nextval('public.cartitems_id_seq'::regclass);


--
-- Name: carts id; Type: DEFAULT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public.carts ALTER COLUMN id SET DEFAULT nextval('public.carts_id_seq'::regclass);


--
-- Name: category id; Type: DEFAULT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public.category ALTER COLUMN id SET DEFAULT nextval('public.category_id_seq'::regclass);


--
-- Name: products id; Type: DEFAULT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public.products ALTER COLUMN id SET DEFAULT nextval('public.products_id_seq'::regclass);


--
-- Name: users id; Type: DEFAULT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public.users ALTER COLUMN id SET DEFAULT nextval('public.users_id_seq'::regclass);


--
-- Data for Name: address; Type: TABLE DATA; Schema: public; Owner: developer
--

--
-- Data for Name: carts; Type: TABLE DATA; Schema: public; Owner: developer
--


--
-- Data for Name: category; Type: TABLE DATA; Schema: public; Owner: developer
--


--
-- Data for Name: products; Type: TABLE DATA; Schema: public; Owner: developer
--


--
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: developer
--


--
-- Name: address_id_seq; Type: SEQUENCE SET; Schema: public; Owner: developer
--

SELECT pg_catalog.setval('public.address_id_seq', 1, false);


--
-- Name: cartitems_id_seq; Type: SEQUENCE SET; Schema: public; Owner: developer
--

SELECT pg_catalog.setval('public.cartitems_id_seq', 7, true);


--
-- Name: carts_id_seq; Type: SEQUENCE SET; Schema: public; Owner: developer
--

SELECT pg_catalog.setval('public.carts_id_seq', 5, true);


--
-- Name: category_id_seq; Type: SEQUENCE SET; Schema: public; Owner: developer
--

SELECT pg_catalog.setval('public.category_id_seq', 5, true);


--
-- Name: products_id_seq; Type: SEQUENCE SET; Schema: public; Owner: developer
--

SELECT pg_catalog.setval('public.products_id_seq', 6, true);


--
-- Name: users_id_seq; Type: SEQUENCE SET; Schema: public; Owner: developer
--

SELECT pg_catalog.setval('public.users_id_seq', 1, true);


--
-- Name: address address_pk; Type: CONSTRAINT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public.address
    ADD CONSTRAINT address_pk PRIMARY KEY (id);


--
-- Name: cartitems cartitems_pk; Type: CONSTRAINT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public.cartitems
    ADD CONSTRAINT cartitems_pk PRIMARY KEY (id);


--
-- Name: carts carts_pk; Type: CONSTRAINT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public.carts
    ADD CONSTRAINT carts_pk PRIMARY KEY (id);


--
-- Name: category category_pk; Type: CONSTRAINT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public.category
    ADD CONSTRAINT category_pk PRIMARY KEY (id);


--
-- Name: products products_pk; Type: CONSTRAINT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public.products
    ADD CONSTRAINT products_pk PRIMARY KEY (id);


--
-- Name: users users_pk; Type: CONSTRAINT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pk PRIMARY KEY (id);


--
-- Name: address address_users_fk; Type: FK CONSTRAINT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public.address
    ADD CONSTRAINT address_users_fk FOREIGN KEY (user_id) REFERENCES public.users(id);


--
-- Name: carts carts_users_fk; Type: FK CONSTRAINT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public.carts
    ADD CONSTRAINT carts_users_fk FOREIGN KEY (user_id) REFERENCES public.users(id);


--
-- Name: products products_category_fk; Type: FK CONSTRAINT; Schema: public; Owner: developer
--

ALTER TABLE ONLY public.products
    ADD CONSTRAINT products_category_fk FOREIGN KEY (category_id) REFERENCES public.category(id);


--
-- PostgreSQL database dump complete
--