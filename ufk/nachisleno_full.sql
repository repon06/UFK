--select * from contract_cparams cc left JOIN land_land l on (l.id=cc.land_id) limit 10;
--select * from land_land limit 10;

SELECT sum(начисл2019),sum(оплач2019)
  from

(SELECT

  ВидРазрешИсп,
  count(*)        AS колво,
  sum(начисл2019) AS начисл2019,
  sum(оплач2019)  AS оплач2019,
--  sum(ОснДолг)    AS ОснДолг,
 -- sum(Пени)       AS Пени,
  sum(ПлощАр)     AS ПлощАр
FROM (

SELECT
  lo.name AS гос_мун,
  lp.id,
  lp.name AS ВидРазрешИсп,
  land.*
FROM
  (
    SELECT
      --/*
      trim(c.actual_use)                        AS ВидФактИсп,
      c.name                                    AS №Договора,
      to_char(c.date_doc, 'DD.MM.YYYY')         AS ДатаДоговора,
      c.date_reg                                AS ДатаРег,
      to_char(c.date_in, 'DD.MM.YYYY')          AS ДатаНачалоПрава,
      to_char(c.date_out, 'DD.MM.YYYY')         AS ДатаКонцаПрава,
      l.cadaster,
      l.address                                 AS АдресУчастка,
      -- , l.sity AS Город, l.region AS Район
      l.area                                    AS ПлощЗУ,
      par.area                                  AS ПлощАр,
      -- ,par.date_in, par.date_out,
      par.upks                                  AS УПКС,
      (par.area * par.upks)                     AS КадастрСтоимПоАрендатору,
      cus.legal                                 AS физ_юр,
      cus.name                                  AS арендатор,
      -- , cus.address, cus.postaddress
      cus.address,
      REPLACE(cus.uid, '  ', ' ')               AS Реквизиты
      --, cus.uid_note                AS КемВыдан


      -- 10 - претензия; -- 85 - взап
      --  , getSummInDocs(c.id , l.id , cus.id , 85) as ЗадолжВЗАП
      --  , getdocs(c.id , l.id , cus.id,85) as ЗадолжВЗАПОпис
      --  , getSummInDocs(c.id , l.id , cus.id , 10) as Претенз
      --  , getdocs(c.id , l.id , cus.id,10) as ПретензОпис
      --  ,getAllDocs(c.id , l.id , cus.id) as История
      --*/par.area                                  AS ПлощАр,





      --, getPayinYaer(c.id,l.id,cus.id,2018,1) AS начисл2018
      --, getPayinYaer(c.id,l.id,cus.id,2018,2) AS оплач2018


      ,getPayinYaer(c.id, l.id, cus.id, 2019, 1) AS начисл2019
      ,getPayinYaer(c.id, l.id, cus.id, 2019, 2) AS оплач2019
      ,getPayinYaer(c.id, l.id, cus.id, 2020, 1) AS начисл2020
      ,getPayinYaer(c.id, l.id, cus.id, 2020, 2) AS оплач2020


      ,getsaldopeni(c.id, l.id, cus.id, 'd')     AS ОснДолг
      ,getsaldopeni(c.id, l.id, cus.id, 'p')     AS Пени
      --,getlastparam(c.id,l.id,cus.id)
      ,
      l.ownership_id,
      l.permitted_id,
      c.id                                         contr_id,
      l.id                                         land_id,
      cus.id                                       cust_id
    FROM contract_contract c
      JOIN contract_cland cl ON (cl.contract_id = c.id)
      JOIN land_land l ON (l.id = cl.land_id)
      JOIN land_permitted lp ON (lp.id = l.permitted_id)
      JOIN contract_ccustomer ccus ON (ccus.contract_id = c.id AND ccus.land_id = l.id)
      JOIN customer_customers cus ON (cus.id = ccus.customer_id)
      JOIN contract_cparams par
        ON (par.contract_id = c.id AND par.land_id = l.id AND par.customer_id = cus.id AND par.date_in =
                                                                                           (SELECT max(date_in)
                                                                                            FROM
                                                                                              contract_cparams cp
                                                                                            WHERE
                                                                                              cp.contract_id =
                                                                                              c.id
                                                                                              AND
                                                                                              cp.customer_id =
                                                                                              cus.id
                                                                                              AND
                                                                                              cp.land_id = l.id))


    WHERE
      1=1
    --  c.state IN (1) -- 1=действующие 0, 1
     -- AND l.state = 1 -- дейтва ЗУ
    --  AND cus.state = 1 -- действующщие арендаторы
      AND cl.state = 1
      AND ccus.state = 1
      AND c.name NOT IN ('Назначение платежа', 'Купля-продажа', 'Идентификация')
      --and c.name in ('852','1416', '2264')
      AND l.ownership_id IN (3, 5) -- !!! ГОС+МУНИЦИП  -- 3=муницип, 5=гос 3,5
      AND c.vidprava_id = 6 -- аренда
      AND cus.legal IN ('u', 'f') -- юр лица 'u', 'f'


    GROUP BY legal, c.id, l.id, cus.id, par.id
    -- and ab.id IN (1209632,1084942,1081399,1209632)

  ) AS land
  JOIN land_ownership lo ON lo.id = land.ownership_id
  JOIN land_permitted lp ON (lp.id = land.permitted_id)
ORDER BY /*land.ОснДолг, land.физ_юр, land.ДатаДоговора,*/ гос_мун, lp.id,
  ДатаДоговора --б land.contr_id, land.cust_id, land.land_id
--LIMIT 500

     ) AS xxx

GROUP BY ВидРазрешИсп
ORDER BY 1)
  AS razrez;

