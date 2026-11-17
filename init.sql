-- Удаляем старые таблицы (и от проекта «Фабрика», и от предыдущего запуска этого скрипта)
DROP TABLE IF EXISTS prodazha CASCADE;
DROP TABLE IF EXISTS firma_toplivo CASCADE;
DROP TABLE IF EXISTS azs CASCADE;
DROP TABLE IF EXISTS klient CASCADE;
DROP TABLE IF EXISTS toplivo CASCADE;
DROP TABLE IF EXISTS firma CASCADE;
DROP TABLE IF EXISTS postuplenie CASCADE;
DROP TABLE IF EXISTS model CASCADE;
DROP TABLE IF EXISTS tovar CASCADE;

CREATE TABLE firma (
    nazvanie_firmy VARCHAR(100) PRIMARY KEY,
    yur_adres VARCHAR(200) NOT NULL,
    telefon VARCHAR(30) NOT NULL
);

CREATE TABLE azs (
    kod_azs SERIAL PRIMARY KEY,
    nazvanie_firmy VARCHAR(100) NOT NULL REFERENCES firma(nazvanie_firmy) ON DELETE RESTRICT,
    adres_azs VARCHAR(200) NOT NULL
);

CREATE TABLE toplivo (
    kod_topliva SERIAL PRIMARY KEY,
    vid_topliva VARCHAR(50) NOT NULL,
    edinica_izmereniya VARCHAR(20) NOT NULL,
    cena_rub NUMERIC(10,2) NOT NULL CHECK (cena_rub > 0)
);

-- Связь «многие ко многим»: какая фирма какое топливо поставляет
CREATE TABLE firma_toplivo (
    nazvanie_firmy VARCHAR(100) NOT NULL REFERENCES firma(nazvanie_firmy) ON DELETE RESTRICT,
    kod_topliva INT NOT NULL REFERENCES toplivo(kod_topliva) ON DELETE RESTRICT,
    PRIMARY KEY (nazvanie_firmy, kod_topliva)
);

CREATE TABLE klient (
    kart_schet VARCHAR(30) PRIMARY KEY,
    fio VARCHAR(150) NOT NULL,
    adres VARCHAR(200) NOT NULL,
    telefon VARCHAR(30) NOT NULL
);

CREATE TABLE prodazha (
    kod_prodazhi SERIAL PRIMARY KEY,
    data_prodazhi DATE NOT NULL CHECK (data_prodazhi <= CURRENT_DATE),
    kart_schet VARCHAR(30) NOT NULL REFERENCES klient(kart_schet) ON DELETE RESTRICT,
    kod_azs INT NOT NULL REFERENCES azs(kod_azs) ON DELETE RESTRICT,
    kod_topliva INT NOT NULL REFERENCES toplivo(kod_topliva) ON DELETE RESTRICT,
    kolichestvo NUMERIC(10,2) NOT NULL CHECK (kolichestvo > 0)
);

CREATE INDEX ix_azs_firma ON azs(nazvanie_firmy);
CREATE INDEX ix_prodazha_klient ON prodazha(kart_schet);
CREATE INDEX ix_prodazha_azs ON prodazha(kod_azs);
CREATE INDEX ix_prodazha_toplivo ON prodazha(kod_topliva);
CREATE INDEX ix_prodazha_data ON prodazha(data_prodazhi);

-- Тестовые данные
INSERT INTO firma (nazvanie_firmy, yur_adres, telefon) VALUES
    ('Лукойл', 'Москва, ул. Нефтяная 1', '8-495-123-45-67'),
    ('Роснефть', 'Москва, Софийская наб. 26/1', '8-495-777-44-44'),
    ('Газпромнефть', 'Санкт-Петербург, ул. Почтамтская 3-5', '8-812-363-31-52'),
    ('Татнефть', 'Альметьевск, ул. Ленина 75', '8-8553-30-50-50'),
    ('Сургутнефтегаз', 'Сургут, ул. Григория Кукуевицкого 1', '8-3462-42-61-33');

INSERT INTO azs (nazvanie_firmy, adres_azs) VALUES
    ('Лукойл', 'Москва, ул. Автозаводская 10'),
    ('Лукойл', 'Москва, Ленинградское ш. 25'),
    ('Роснефть', 'Москва, ул. Профсоюзная 56'),
    ('Роснефть', 'Санкт-Петербург, Московский пр. 100'),
    ('Газпромнефть', 'Москва, Варшавское ш. 12'),
    ('Газпромнефть', 'Казань, ул. Чистопольская 5'),
    ('Татнефть', 'Казань, ул. Бутлерова 30'),
    ('Сургутнефтегаз', 'Сургут, ул. Энгельса 8');

INSERT INTO toplivo (vid_topliva, edinica_izmereniya, cena_rub) VALUES
    ('АИ-92', 'литр', 47.50),
    ('АИ-95', 'литр', 52.30),
    ('АИ-98', 'литр', 63.80),
    ('Дизельное топливо', 'литр', 58.10),
    ('Газ СУГ', 'литр', 28.40),
    ('АИ-100', 'литр', 71.20);

INSERT INTO firma_toplivo (nazvanie_firmy, kod_topliva) VALUES
    ('Лукойл', 1), ('Лукойл', 2), ('Лукойл', 3), ('Лукойл', 4),
    ('Роснефть', 1), ('Роснефть', 2), ('Роснефть', 4), ('Роснефть', 5),
    ('Газпромнефть', 1), ('Газпромнефть', 2), ('Газпромнефть', 3), ('Газпромнефть', 4), ('Газпромнефть', 6),
    ('Татнефть', 1), ('Татнефть', 2), ('Татнефть', 4),
    ('Сургутнефтегаз', 1), ('Сургутнефтегаз', 2), ('Сургутнефтегаз', 4), ('Сургутнефтегаз', 5);

INSERT INTO klient (kart_schet, fio, adres, telefon) VALUES
    ('1000 2000 3000 0001', 'Иванов Иван Иванович', 'Москва, ул. Ленина 10', '8-495-111-22-33'),
    ('1000 2000 3000 0002', 'Петров Пётр Петрович', 'Москва, ул. Тверская 5', '8-495-222-33-44'),
    ('1000 2000 3000 0003', 'Сидорова Анна Сергеевна', 'Санкт-Петербург, Невский пр. 20', '8-812-333-44-55'),
    ('1000 2000 3000 0004', 'Кузнецов Алексей Викторович', 'Казань, ул. Баумана 7', '8-843-444-55-66'),
    ('1000 2000 3000 0005', 'Смирнова Ольга Николаевна', 'Москва, ул. Арбат 15', '8-495-555-66-77'),
    ('1000 2000 3000 0006', 'Попов Дмитрий Андреевич', 'Сургут, ул. Мира 3', '8-3462-66-77-88'),
    ('1000 2000 3000 0007', 'Васильев Сергей Олегович', 'Казань, ул. Кремлёвская 9', '8-843-777-88-99'),
    ('1000 2000 3000 0008', 'Морозова Елена Павловна', 'Москва, ул. Садовая 1', '8-495-888-99-00');

INSERT INTO prodazha (data_prodazhi, kart_schet, kod_azs, kod_topliva, kolichestvo) VALUES
    ('2026-10-01', '1000 2000 3000 0001', 1, 2, 40),
    ('2026-10-01', '1000 2000 3000 0002', 3, 1, 35),
    ('2026-10-02', '1000 2000 3000 0003', 4, 4, 60),
    ('2026-10-03', '1000 2000 3000 0001', 2, 1, 25),
    ('2026-10-04', '1000 2000 3000 0004', 6, 2, 45),
    ('2026-10-05', '1000 2000 3000 0005', 5, 3, 30),
    ('2026-10-06', '1000 2000 3000 0004', 7, 1, 50),
    ('2026-10-07', '1000 2000 3000 0006', 8, 4, 70),
    ('2026-10-08', '1000 2000 3000 0002', 1, 4, 55),
    ('2026-10-09', '1000 2000 3000 0007', 7, 2, 38),
    ('2026-10-10', '1000 2000 3000 0003', 3, 5, 42),
    ('2026-10-11', '1000 2000 3000 0005', 2, 3, 28.5),
    ('2026-10-12', '1000 2000 3000 0006', 8, 5, 33),
    ('2026-10-13', '1000 2000 3000 0001', 5, 2, 47),
    ('2026-10-14', '1000 2000 3000 0007', 6, 4, 65);
