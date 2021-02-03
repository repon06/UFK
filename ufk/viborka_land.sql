select lo.name AS гос_мун, land.*  FROM
  (
    SELECT
      c.name                      AS №Договора,
      to_char(c.date_doc,'DD.MM.YYYY')                  AS ДатаДоговора,
      c.date_reg as ДатаРег,
      to_char(c.date_in,'DD.MM.YYYY') as ДатаНачалоПрава,
      to_char(c.date_out,'DD.MM.YYYY') as ДатаКонцаПрава,
      l.cadaster,
      l.address                   AS АдресУчастка, -- , l.sity AS Город, l.region AS Район
      l.area                      AS ПлощЗУ,
      par.area as ПлощАр, -- ,par.date_in, par.date_out,
      cus.legal as физ_юр,
      cus.name                    AS арендатор, -- , cus.address, cus.postaddress
      cus.address,
      REPLACE(cus.uid, '  ', ' ') AS Паспорт
      --, cus.uid_note                AS КемВыдан




   -- 10 - претензия; -- 85 - взап
    , getSummInDocs(c.id , l.id , cus.id , 85) as ЗадолжВЗАП
    , getdocs(c.id , l.id , cus.id,85) as ЗадолжВЗАПОпис
    , getSummInDocs(c.id , l.id , cus.id , 10) as Претенз
    , getdocs(c.id , l.id , cus.id,10) as ПретензОпис
--, ccus.id
 -- , ccus.osn_dolg_in AS сальдо_из_дог
 -- , ccus.peni_in AS пени_из_дог
 -- , ccus.start_date AS сальдо_дата
 ,getsaldopeni(c.id,l.id, cus.id, 'd')  as ОснДолг
 ,getsaldopeni(c.id,l.id, cus.id, 'p') as Пени
            ,l.ownership_id
            ,c.id contr_id, l.id land_id, cus.id cust_id
    FROM contract_contract c
      JOIN contract_cland cl ON (cl.contract_id = c.id)
      JOIN land_land l ON (l.id = cl.land_id)
      JOIN land_permitted lp ON (lp.id = l.permitted_id)
      JOIN contract_ccustomer ccus ON (ccus.contract_id = c.id AND ccus.land_id = l.id)
      JOIN customer_customers cus ON (cus.id = ccus.customer_id)
      JOIN contract_cparams par ON (par.contract_id=c.id AND par.land_id=l.id and par.customer_id=cus.id and par.date_in =
                            (SELECT max(date_in) from contract_cparams cp where cp.contract_id=c.id and cp.customer_id=cus.id and cp.land_id=l.id))
      --JOIN land_ownership lo ON lo.id = l.ownership_id
    --   JOIN arenda_balans ab ON (ab.contract_id=c.id AND ab.land_id=l.id AND ab.customer_id=cus.id)
    -- JOIN contract_cparams par ON (par.contract_id=c.id AND par.land_id=l.id and par.customer_id=cus.id)
    -- LEFT JOIN land_permittedsub lps ON lps.id=l.subpermitted_id
    -- LEFT JOIN doc_doc d ON (d.contract_id=c.id AND d.land_id=l.id AND d.customer_id=cus.id AND d.vid_id=9) -- 9-соглашение
    -- LEFT JOIN doc_vid dv ON (dv.id=d.vid_id AND dv.id=9) -- 9-соглашение

    WHERE
          c.state IN (1) -- 1=действующие 0, 1
      AND l.state = 1 -- дейтва ЗУ
      AND cus.state = 1 -- действующщие арендаторы
      AND cl.state = 1
      AND ccus.state = 1
      AND c.name NOT IN ('Назначение платежа', 'Купля-продажа', 'Идентификация')
      AND l.ownership_id IN (3, 5) -- !!! ГОС+МУНИЦИП  -- 3=муницип, 5=гос 3,5
      AND c.vidprava_id = 6 -- аренда
      AND cus.legal IN ('u', 'f') -- юр лица 'u', 'f'

  AND c.id in(508789 ,507171,507275,501719,500501,509191,509324,510014,510247,510138,510046,6363,3405,67)
  --and cus.id=503163
  --and l.id=511833

    -- and ab.oper = 1 -- 1-начислено 2-оплачено
    -- AND ab.state = 1 -- 1=действующие начисления
    -- and ab.year = 2015
    --  AND '2016.02.01' between ab.date_in and ab.date_out
    -- and c.date_in > '2000.01.01'
    -- AND c.date_doc >= '2015.01.01' -- начинаются до
    -- and par.bonus_coeff IN ( 0.01 , 0.0)
    -- and dv.id IN (41,42,74) -- ДОК-ты с РАСТОРЖЕНИЕМ
    -- and dv.id IN (9) -- ДОК-ты с соглашением
    -- -- -- --  and c.date_doc BETWEEN '2015.01.01' AND '2015.09.16' -- заключены за промежуток
    -- AND c.date_out >= '2013.12.31' -- заканчиваются после ДАТУ о
    --  and ccus.state=1 AND cus.state=1
    -- AND (par.bonus_coeff < 1 / *and par.date_in>'2015.01.01' AND par.date_out<'2015.12.31'*/) --
    -- AND c.id=5900
    --  and c.name in ('Ар-15-205/Ю-2')
    -- and c.id=510186 -- contract_id
    -- AND lp.id IN (1,5,7,9) AND c.actual_use LIKE '%строител%' -- вид факт и разреш использования
    -- and LOWER(l.address) LIKE LOWER('%Песч%ум%10%')
    -- and cus.id = 1851 -- саратовводоканал
    -- and l.address LIKE '%Кокурино%'
    -- AND (l.subpermitted_id is NULL OR l.subpermitted_id IN (2, 302) )
    -- and l.subpermitted_id IN ( 2)
    -- and l.subpermitted_id is not NULL
    /*
  AND (cus.legal IN ( 'u' ) AND cus.uid NOT LIKE '%пасп%' AND cus.uid NOT LIKE '%павп%'
        and cus.uid NOT LIKE '%свидетельство о рождении%' AND cus.uid <> 'ИНН: -    /'
        and cus.uid <> '' AND cus.uid NOT LIKE '%снилс%') -- юр лица 'u', 'f'
  */
    GROUP BY legal, c.id, l.id, cus.id, par.id
    -- and ab.id IN (1209632,1084942,1081399,1209632)

  ) as land
JOIN land_ownership lo ON lo.id = land.ownership_id
ORDER BY land.ОснДолг, land.физ_юр, land.ДатаДоговора, land.contr_id, land.cust_id, land.land_id
;


/*
select * ,  --getTest(ccus.contract_id,ccus.land_id, ccus.customer_id)
getsaldopeni(ccus.contract_id,ccus.land_id, ccus.customer_id, 'p')
from contract_ccustomer ccus where ccus.contract_id=508789 and ccus.land_id=512262 and state=1;


