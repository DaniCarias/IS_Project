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

ALTER TABLE ONLY public.subscription DROP CONSTRAINT subscription_container_id_fk;
ALTER TABLE ONLY public.data DROP CONSTRAINT data_container_id_fk;
ALTER TABLE ONLY public.container DROP CONSTRAINT container_application_id_fk;
ALTER TABLE ONLY public.subscription DROP CONSTRAINT subscription_pk_2;
ALTER TABLE ONLY public.subscription DROP CONSTRAINT subscription_pk;
ALTER TABLE ONLY public.data DROP CONSTRAINT data_pk;
ALTER TABLE ONLY public.container DROP CONSTRAINT container_pk_2;
ALTER TABLE ONLY public.container DROP CONSTRAINT container_pk;
ALTER TABLE ONLY public.application DROP CONSTRAINT application_pk_2;
ALTER TABLE ONLY public.application DROP CONSTRAINT application_pk;
DROP TABLE public.subscription;
DROP TABLE public.data;
DROP TABLE public.container;
DROP TABLE public.application;
DROP SCHEMA public;
--
-- Name: public; Type: SCHEMA; Schema: -; Owner: pg_database_owner
--

CREATE SCHEMA public;


ALTER SCHEMA public OWNER TO pg_database_owner;

--
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: pg_database_owner
--

COMMENT ON SCHEMA public IS 'standard public schema';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: application; Type: TABLE; Schema: public; Owner: projeto_is
--

CREATE TABLE public.application (
    id integer NOT NULL,
    name character varying NOT NULL,
    creation_dt timestamp without time zone DEFAULT now() NOT NULL
);


ALTER TABLE public.application OWNER TO projeto_is;

--
-- Name: container; Type: TABLE; Schema: public; Owner: projeto_is
--

CREATE TABLE public.container (
    id integer NOT NULL,
    name character varying NOT NULL,
    creation_dt timestamp without time zone DEFAULT now() NOT NULL,
    parent integer
);


ALTER TABLE public.container OWNER TO projeto_is;

--
-- Name: data; Type: TABLE; Schema: public; Owner: projeto_is
--

CREATE TABLE public.data (
    id integer NOT NULL,
    content character varying NOT NULL,
    creation_dt timestamp without time zone DEFAULT now() NOT NULL,
    parent integer
);


ALTER TABLE public.data OWNER TO projeto_is;

--
-- Name: subscription; Type: TABLE; Schema: public; Owner: projeto_is
--

CREATE TABLE public.subscription (
    id integer NOT NULL,
    name character varying NOT NULL,
    creation_dt timestamp without time zone DEFAULT now(),
    event character varying,
    endpoint character varying,
    parent integer,
    CONSTRAINT event_check CHECK (((event)::text = ANY (ARRAY[('creation'::character varying)::text, ('deletion'::character varying)::text])))
);


ALTER TABLE public.subscription OWNER TO projeto_is;

--
-- Data for Name: application; Type: TABLE DATA; Schema: public; Owner: projeto_is
--

COPY public.application (id, name, creation_dt) FROM stdin;
\.


--
-- Data for Name: container; Type: TABLE DATA; Schema: public; Owner: projeto_is
--

COPY public.container (id, name, creation_dt, parent) FROM stdin;
\.


--
-- Data for Name: data; Type: TABLE DATA; Schema: public; Owner: projeto_is
--

COPY public.data (id, content, creation_dt, parent) FROM stdin;
\.


--
-- Data for Name: subscription; Type: TABLE DATA; Schema: public; Owner: projeto_is
--

COPY public.subscription (id, name, creation_dt, event, endpoint, parent) FROM stdin;
\.


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
-- Name: data data_container_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: projeto_is
--

ALTER TABLE ONLY public.data
    ADD CONSTRAINT data_container_id_fk FOREIGN KEY (parent) REFERENCES public.container(id);


--
-- Name: subscription subscription_container_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: projeto_is
--

ALTER TABLE ONLY public.subscription
    ADD CONSTRAINT subscription_container_id_fk FOREIGN KEY (parent) REFERENCES public.container(id);


--
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: pg_database_owner
--

REVOKE USAGE ON SCHEMA public FROM PUBLIC;


--
-- PostgreSQL database dump complete
--

