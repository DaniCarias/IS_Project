--
-- PostgreSQL database dump
--

-- Dumped from database version 16.1 (Ubuntu 16.1-1.pgdg22.04+1)
-- Dumped by pg_dump version 16.1 (Ubuntu 16.1-1.pgdg22.04+1)

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

ALTER TABLE ONLY public.subscription DROP CONSTRAINT subscription_container_fk;
ALTER TABLE ONLY public.data DROP CONSTRAINT data_container_fk;
ALTER TABLE ONLY public.container DROP CONSTRAINT container_application_id_fk;
ALTER TABLE ONLY public.subscription DROP CONSTRAINT subscription_pk_2;
ALTER TABLE ONLY public.subscription DROP CONSTRAINT subscription_pk;
ALTER TABLE ONLY public.data DROP CONSTRAINT data_pk;
ALTER TABLE ONLY public.container DROP CONSTRAINT container_pk_2;
ALTER TABLE ONLY public.container DROP CONSTRAINT container_pk;
ALTER TABLE ONLY public.application DROP CONSTRAINT application_pk_2;
ALTER TABLE ONLY public.application DROP CONSTRAINT application_pk;
ALTER TABLE public.subscription ALTER COLUMN id DROP DEFAULT;
ALTER TABLE public.data ALTER COLUMN id DROP DEFAULT;
ALTER TABLE public.container ALTER COLUMN id DROP DEFAULT;
ALTER TABLE public.application ALTER COLUMN id DROP DEFAULT;
DROP SEQUENCE public.subscription_id_seq;
DROP TABLE public.subscription;
DROP SEQUENCE public.data_id_seq;
DROP TABLE public.data;
DROP SEQUENCE public.container_id_seq;
DROP TABLE public.container;
DROP SEQUENCE public.application_id_seq;
DROP TABLE public.application;
SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: application; Type: TABLE; Schema: public; Owner: projeto_is
--

CREATE TABLE public.application (
    name character varying NOT NULL,
    creation_dt timestamp without time zone DEFAULT now(),
    id bigint NOT NULL
);


ALTER TABLE public.application OWNER TO projeto_is;

--
-- Name: application_id_seq; Type: SEQUENCE; Schema: public; Owner: projeto_is
--

CREATE SEQUENCE public.application_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.application_id_seq OWNER TO projeto_is;

--
-- Name: application_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: projeto_is
--

ALTER SEQUENCE public.application_id_seq OWNED BY public.application.id;


--
-- Name: container; Type: TABLE; Schema: public; Owner: projeto_is
--

CREATE TABLE public.container (
    name character varying NOT NULL,
    creation_dt timestamp without time zone DEFAULT now(),
    parent integer,
    id bigint NOT NULL
);


ALTER TABLE public.container OWNER TO projeto_is;

--
-- Name: container_id_seq; Type: SEQUENCE; Schema: public; Owner: projeto_is
--

CREATE SEQUENCE public.container_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.container_id_seq OWNER TO projeto_is;

--
-- Name: container_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: projeto_is
--

ALTER SEQUENCE public.container_id_seq OWNED BY public.container.id;


--
-- Name: data; Type: TABLE; Schema: public; Owner: projeto_is
--

CREATE TABLE public.data (
    content character varying NOT NULL,
    creation_dt timestamp without time zone DEFAULT now(),
    parent integer,
    id bigint NOT NULL
);


ALTER TABLE public.data OWNER TO projeto_is;

--
-- Name: data_id_seq; Type: SEQUENCE; Schema: public; Owner: projeto_is
--

CREATE SEQUENCE public.data_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.data_id_seq OWNER TO projeto_is;

--
-- Name: data_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: projeto_is
--

ALTER SEQUENCE public.data_id_seq OWNED BY public.data.id;


--
-- Name: subscription; Type: TABLE; Schema: public; Owner: projeto_is
--

CREATE TABLE public.subscription (
    name character varying NOT NULL,
    creation_dt timestamp without time zone DEFAULT now(),
    event character varying,
    endpoint character varying,
    parent integer,
    id bigint NOT NULL,
    CONSTRAINT event_check CHECK (((event)::text = ANY (ARRAY[('creation'::character varying)::text, ('deletion'::character varying)::text])))
);


ALTER TABLE public.subscription OWNER TO projeto_is;

--
-- Name: subscription_id_seq; Type: SEQUENCE; Schema: public; Owner: projeto_is
--

CREATE SEQUENCE public.subscription_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.subscription_id_seq OWNER TO projeto_is;

--
-- Name: subscription_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: projeto_is
--

ALTER SEQUENCE public.subscription_id_seq OWNED BY public.subscription.id;


--
-- Name: application id; Type: DEFAULT; Schema: public; Owner: projeto_is
--

ALTER TABLE ONLY public.application ALTER COLUMN id SET DEFAULT nextval('public.application_id_seq'::regclass);


--
-- Name: container id; Type: DEFAULT; Schema: public; Owner: projeto_is
--

ALTER TABLE ONLY public.container ALTER COLUMN id SET DEFAULT nextval('public.container_id_seq'::regclass);


--
-- Name: data id; Type: DEFAULT; Schema: public; Owner: projeto_is
--

ALTER TABLE ONLY public.data ALTER COLUMN id SET DEFAULT nextval('public.data_id_seq'::regclass);


--
-- Name: subscription id; Type: DEFAULT; Schema: public; Owner: projeto_is
--

ALTER TABLE ONLY public.subscription ALTER COLUMN id SET DEFAULT nextval('public.subscription_id_seq'::regclass);


--
-- Data for Name: application; Type: TABLE DATA; Schema: public; Owner: projeto_is
--

COPY public.application (name, creation_dt, id) FROM stdin;
Lamp	2023-12-28 14:32:19.43594	1
\.


--
-- Data for Name: container; Type: TABLE DATA; Schema: public; Owner: projeto_is
--

COPY public.container (name, creation_dt, parent, id) FROM stdin;
\.


--
-- Data for Name: data; Type: TABLE DATA; Schema: public; Owner: projeto_is
--

COPY public.data (content, creation_dt, parent, id) FROM stdin;
\.


--
-- Data for Name: subscription; Type: TABLE DATA; Schema: public; Owner: projeto_is
--

COPY public.subscription (name, creation_dt, event, endpoint, parent, id) FROM stdin;
\.


--
-- Name: application_id_seq; Type: SEQUENCE SET; Schema: public; Owner: projeto_is
--

SELECT pg_catalog.setval('public.application_id_seq', 1, true);


--
-- Name: container_id_seq; Type: SEQUENCE SET; Schema: public; Owner: projeto_is
--

SELECT pg_catalog.setval('public.container_id_seq', 1, false);


--
-- Name: data_id_seq; Type: SEQUENCE SET; Schema: public; Owner: projeto_is
--

SELECT pg_catalog.setval('public.data_id_seq', 1, false);


--
-- Name: subscription_id_seq; Type: SEQUENCE SET; Schema: public; Owner: projeto_is
--

SELECT pg_catalog.setval('public.subscription_id_seq', 1, false);


--
-- Name: application application_pk; Type: CONSTRAINT; Schema: public; Owner: projeto_is
--

ALTER TABLE ONLY public.application
    ADD CONSTRAINT application_pk PRIMARY KEY (id);


--
-- Name: application application_pk_2; Type: CONSTRAINT; Schema: public; Owner: projeto_is
--

ALTER TABLE ONLY public.application
    ADD CONSTRAINT application_pk_2 UNIQUE (name);


--
-- Name: container container_pk; Type: CONSTRAINT; Schema: public; Owner: projeto_is
--

ALTER TABLE ONLY public.container
    ADD CONSTRAINT container_pk PRIMARY KEY (id);


--
-- Name: container container_pk_2; Type: CONSTRAINT; Schema: public; Owner: projeto_is
--

ALTER TABLE ONLY public.container
    ADD CONSTRAINT container_pk_2 UNIQUE (name);


--
-- Name: data data_pk; Type: CONSTRAINT; Schema: public; Owner: projeto_is
--

ALTER TABLE ONLY public.data
    ADD CONSTRAINT data_pk PRIMARY KEY (id);


--
-- Name: subscription subscription_pk; Type: CONSTRAINT; Schema: public; Owner: projeto_is
--

ALTER TABLE ONLY public.subscription
    ADD CONSTRAINT subscription_pk PRIMARY KEY (id);


--
-- Name: subscription subscription_pk_2; Type: CONSTRAINT; Schema: public; Owner: projeto_is
--

ALTER TABLE ONLY public.subscription
    ADD CONSTRAINT subscription_pk_2 UNIQUE (name);


--
-- Name: container container_application_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: projeto_is
--

ALTER TABLE ONLY public.container
    ADD CONSTRAINT container_application_id_fk FOREIGN KEY (parent) REFERENCES public.application(id);


--
-- Name: data data_container_fk; Type: FK CONSTRAINT; Schema: public; Owner: projeto_is
--

ALTER TABLE ONLY public.data
    ADD CONSTRAINT data_container_fk FOREIGN KEY (parent) REFERENCES public.container(id);


--
-- Name: subscription subscription_container_fk; Type: FK CONSTRAINT; Schema: public; Owner: projeto_is
--

ALTER TABLE ONLY public.subscription
    ADD CONSTRAINT subscription_container_fk FOREIGN KEY (parent) REFERENCES public.container(id);


--
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: pg_database_owner
--

REVOKE USAGE ON SCHEMA public FROM PUBLIC;


--
-- PostgreSQL database dump complete
--

