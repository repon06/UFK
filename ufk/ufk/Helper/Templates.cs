using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ufk.Helper
{
    class PaymentFKValues
    {
        private string fk { get; set; }
        public Dictionary<string, string> FK { set; get; }
        private string bd { get; set; }
        public Dictionary<string, string> BD { set; get; }
        private List<Payment> payments { get; set; }
        //private Dictionary<string, string> PAUMENTS_DIC { set; get; }//в один дикт вносим/парсим bdpd и bdpdst
        public List<Dictionary<string, string>> PAUMENTS = new List<Dictionary<string, string>>(); //{ set; get; }//генерим список из распарс ДИКШН PAUMENTS_DIC

        public PaymentFKValues()
        { }

        public PaymentFKValues(string fileContent)
        {
                var fk_regex_pt = new Regex(@"FK\|.*\n");
                var bd_regex_pt = new Regex(@"BD\|.*\n");
                var payments_regex_pt = new Regex(@"(BDPD\|.*(\n|$))(BDPDST\|.*(\n|$))");

                var fk_match = fk_regex_pt.Match(fileContent);
                var bd_match = bd_regex_pt.Match(fileContent);
                var payment_matches = payments_regex_pt.Matches(fileContent);

                fk = fk_match.Value;
                bd = bd_match.Value;
                var payment_pd_pdst = payment_matches.Cast<Match>().Select(match => match.Value).ToList();//выборка по 2 строки одного платежа
                payments = payment_pd_pdst.Cast<string>().Select(paym => new Payment(paym)).ToList();

                var paymentHelper = new PaymentHelper();

                FK = paymentHelper.ParsePayment(fk, PaymentType.fk);
                BD = paymentHelper.ParsePayment(bd, PaymentType.bd);

                foreach (Payment key_val in payments)
                {
                    Dictionary<string, string> bdpd = paymentHelper.ParsePayment(key_val.bdpd, PaymentType.bdpd);
                    Dictionary<string, string> bdpdst = paymentHelper.ParsePayment(key_val.bdpdst, PaymentType.bdpdst);
                 
                    //ищем разные value с одинаковым ключом
                    var intersectedItems = bdpd.Where(x => bdpdst.ContainsKey(x.Key) && !bdpdst.ContainsValue(x.Value)).Select(x => new
                    {
                        Key = x.Key,
                        Value = x.Value + "; " + bdpdst[x.Key]
                     }).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                     if (intersectedItems.Any())
                           throw new Exception($"В реестре платежка: bdpd|{bdpd["NOM_EL_MES"]} имеет различные значения [{intersectedItems.Keys.Aggregate((i, j) => i + ", " + j) }] : [{intersectedItems.Values.Aggregate((i, j) => i + ", " + j)}] ");

                    //может выдать ошибку "элемент с тем же ключом уже был добавлен"
                    Dictionary<string, string> paym_dic = bdpd.Union(bdpdst).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);//объединяем

                    PAUMENTS.Add(paym_dic);
                }


                /* можно так
                payment = new List<Payment>();

                foreach (string paym in payment_)
                    payment.Add(new Payment(paym));
                */
            

        }


    }

    /// <summary>
    /// последовательные записи 1 платежки
    /// </summary>
    class Payment
    {
        public Payment(string paymentContent)
        {
            var bdpd_regex_pt = new Regex(@"(BDPD\|.*(\n|$))");
            var bdpdst_regex_pt = new Regex(@"(BDPDST\|.*(\n|$))");

            var bdpd_match = bdpd_regex_pt.Match(paymentContent);
            var bdpdst_match = bdpdst_regex_pt.Match(paymentContent);

            bdpd = bdpd_match.Value;
            bdpdst = bdpdst_match.Value;
        }

        public string bdpd { get; set; }
        public string bdpdst { get; set; }
    }

    /// <summary>
    /// для выбора типа парамента платежа
    /// при парсинге
    /// </summary>
    enum PaymentType
    {
        fk, bd, bdpd, bdpdst
    }

    class PaymentFKTemplate
    {
        public PaymentFKTemplate(string fk_, string bd_, string bdpd_, string bdpdst_) { fk = fk_; bd = bd_; bdpd = bdpd_; bdpdst = bdpdst_; }
        public string fk { get; set; }
        public string bd { get; set; }
        public string bdpd { get; set; }
        public string bdpdst { get; set; }
    }

    /// <summary>
    /// первая общ строка платежки
    /// </summary>
    class FK
    {
        public string NUM_VER { get; set; }
        public string FORMER { get; set; }
        public string FORM_VER { get; set; }
        public string NORM_DOC { get; set; }
    }

    /// <summary>
    /// втрая общ строка платежки
    /// </summary>
    class BD
    {
        public string LS { get; set; }
        public string DATE { get; set; }
        public string GUID_OTCH { get; set; }
        public string KOD_DOC { get; set; }
        public string VID_OTCH { get; set; }
        public string KOL { get; set; }
        public string SUM_TOTAL { get; set; }

    }

    /// <summary>
    /// Информация из расчетных документов
    /// </summary>
    class BDPD
    {
        public string NOM_EL_MES { get; set; }
        public string DATE_EL_MES { get; set; }
        public string ID_EL_MES { get; set; }
        public string NUM_PP { get; set; }
        public string DATE_PP { get; set; }
        public string SUM_PP { get; set; }
        public string VID_PL { get; set; }
        public string DATE_PP_IN { get; set; }
        public string DATE_PP_SPS { get; set; }
        public string VID_OPER { get; set; }
        public string INN_PAY { get; set; }
        public string KPP_PAY { get; set; }
        public string CNAME_PAY { get; set; }
        public string BS_PAY { get; set; }
        public string BIC_PAY { get; set; }
        public string NAME_BIC_PAY { get; set; }
        public string BS_KS_PAY { get; set; }
        public string INN_RCP { get; set; }
        public string KPP_RCP { get; set; }
        public string СNAME_UBP_RCP { get; set; }
        public string BS_RCP { get; set; }
        public string BIC_RCP { get; set; }
        public string NAME_BIC_RCP { get; set; }
        public string BS_KS_RCP { get; set; }
        public string DATE_PAY { get; set; }
        public string ORDER_PAY { get; set; }
        public string UIN { get; set; }
        public string PURPOSE { get; set; }
        public string PAYSTATUS { get; set; }
        public string KBK { get; set; }
        public string OKATO { get; set; }
        public string OSNPLAT { get; set; }
        public string NAL_PER { get; set; }
        public string NUM_DOK { get; set; }
        public string DATE_DOK { get; set; }
        public string TYPE_PL { get; set; }
        public string NOM_PL_PO { get; set; }
        public string SHIFR_RD_PO { get; set; }
        public string NOM_RD_PO { get; set; }
        public string DATE_RD_PO { get; set; }
        public string SUM_OST_PO { get; set; }
        public string OPER_PO { get; set; }
        public string DATE_IN_TOFK { get; set; }
        public string GUID { get; set; }
        public string ID_CONTR { get; set; }
        public string NUM_AKK { get; set; }
        public string SUM_NDS_ITOG { get; set; }
    }

    class BDPDST
    {
        public string KBK { get; set; }
        public string TYPE_KBK { get; set; }
        public string ADD_KLASS { get; set; }
        public string NUM_BO { get; set; }
        public string ОКАТО { get; set; }
        public string SUM { get; set; }
        public string SUM_NDS { get; set; }
        public string DIR_SUM { get; set; }
        public string MES_FIN { get; set; }
        public string REZERV { get; set; }
    }

    /*
Описание блока/поля|Имя блока/поля|Тип
Почтовая информация об отправителе файла|FROM|
Код ОрФК|KOD_TOFK|STRING
Наименование ОрФК|NAME_TOFK|STRING
Почтовая информация о получателе файла|TO|
Код ОрФК|KOD_TOFK|STRING
Наименование ОрФК|NAME_TOFK|STRING
Уровень бюджета|BUDG_LEVEL|STRING
Код клиента|KOD_UBP|STRING
Наименование клиента|NAME_UBP|STRING
Общая информация|BD|
Номер л/с клиента|LS|STRING
Дата документа, к которому прилагается информация из расчетных документов|DATE|DATE
Глобальный уникальный идентификатор|GUID_OTCH|GUID
Код документа, к которому прилагается информация из расчетных документов|KOD_DOC|STRING
Признак принадлежности к промежуточному отчёту|VID_OTCH|STRING
Количество документов|KOL|STRING
Контрольная сумма по документам|SUM_TOTAL|NUMBER2
Информация из расчетных документов|BDPD(*)|
Порядковый номер ЭС|NOM_EL_MES|STRING
Дата составления ЭС|DATE_EL_MES|DATE
Уникальный идентификатор составителя ЭС|ID_EL_MES|STRING
№ платежного документа|NUM_PP|STRING
Дата платежного документа|DATE_PP|DATE
Сумма платежного поручения|SUM_PP|NUMBER2
Вид платежа:|VID_PL|STRING
Поступило в банк плательщика|DATE_PP_IN|DATE
Дата списания со счета плательщика.|DATE_PP_SPS|DATE
Вид операции|VID_OPER|STRING
ИНН Плательщика|INN_PAY|STRING
КПП Плательщика|KPP_PAY|STRING
Наименование Плательщика|CNAME_PAY|STRING
Расчетный счет Плательщика|BS_PAY|STRING
БИК банка Плательщика|BIC_PAY|STRING
Наименование банка Плательщика|NAME_BIC_PAY|STRING
Коррсчет банка Плательщика в РКЦ|BS_KS_PAY|STRING
ИНН Получателя|INN_RCP|STRING
КПП Получателя|KPP_RCP|STRING
Наименование Получателя|СNAME_UBP_RCP|STRING
Расчетный счет Получателя|BS_RCP|STRING
БИК банка Получателя|BIC_RCP|STRING
Наименование банка Получателя|NAME_BIC_RCP|STRING
Коррсчет банка Получателя в РКЦ|BS_KS_RCP|STRING
Срок платежа|DATE_PAY|DATE
Очередность платежа|ORDER_PAY|STRING
Уникальный идентификатор начисления (Код)|UIN|STRING
Назначение платежа|PURPOSE|STRING
Статус составителя расчетного документа|PAYSTATUS|STRING
Код бюджетной классификации|KBK|STRING
Код ОКТМО|OKATO|STRING
Показатель основания платежа|OSNPLAT|STRING
Показатель налогового периода/ кода таможенного органа|NAL_PER|STRING
Показатель номера документа|NUM_DOK|STRING
Показатель даты документа|DATE_DOK|STRING
Тип платежа|TYPE_PL|STRING
Номер частичного платежа|NOM_PL_PO|STRING
Шифр платежного документа|SHIFR_RD_PO|STRING
Номер платежного документа|NOM_RD_PO|STRING
Дата платежного документа|DATE_RD_PO|DATE
Сумма остатка платежа|SUM_OST_PO|NUMBER2
Содержание операции|OPER_PO|STRING
Дата выписки|DATE_IN_TOFK|DATE
Глобальный идентификатор|GUID|GUID
Идентификатор контракта|ID_CONTR|STRING
Номер аккредитива|NUM_AKK|STRING
Сумма НДС, итого|SUM_NDS_ITOG|NUMBER2
Учетные данные ОрФК. Строки платежного документа по бюджетной классификации|BDPDST(*)|
Код по КБК|KBK|STRING
Тип КБК|TYPE_KBK|STRING
Код цели|ADD_KLASS|STRING
Номер бюджетного обязательства|NUM_BO|STRING
ОКТМО|ОКАТО|STRING
Сумма|SUM|NUMBER2
Сумма НДС|SUM_NDS|NUMBER2
Направление платежа|DIR_SUM|STRING
Месяц финансирования|MES_FIN|STRING
Резервное поле|REZERV|STRING

*/

}
