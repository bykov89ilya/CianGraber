using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace CianGraber
{
    class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        

        private static readonly List<District> _MoscowDistrictsAfter2010 = new List<District>()
        {
            new District { Name = "Внуковское"                  ,        EstimatedPrice = 195500, Region = "НАО"  },
            new District { Name = "Воскресенское"               ,        EstimatedPrice = 170000, Region = "НАО"  },
            new District { Name = "Десёновское"                 ,        EstimatedPrice = 163000, Region = "НАО"  },
            new District { Name = "Кокошкино"                   ,        EstimatedPrice = 135000, Region = "НАО"  },
            new District { Name = "Марушкинское"                ,        EstimatedPrice = 132000, Region = "НАО"  },
            new District { Name = "Московский"                  ,        EstimatedPrice = 194666, Region = "НАО"  },
            new District { Name = "Мосрентген"                  ,        EstimatedPrice = 166000, Region = "НАО"  },
            new District { Name = "Рязановское"                 ,        EstimatedPrice = 157000, Region = "НАО"  },
            new District { Name = "Сосенское"                   ,        EstimatedPrice = 183750, Region = "НАО"  },
            new District { Name = "Филимонковское"              ,        EstimatedPrice = 116000, Region = "НАО"  },
            new District { Name = "Щербинка"                    ,        EstimatedPrice = 160000, Region = "НАО"  },
            
            new District { Name = "Куркино"                     ,        EstimatedPrice = 220000, Region = "СЗАО" },
            new District { Name = "Митино"                      ,        EstimatedPrice = 191000, Region = "СЗАО" },
            new District { Name = "Покровское-Стрешнево"        ,        EstimatedPrice = 276000, Region = "СЗАО" },
            new District { Name = "Северное Тушино"             ,        EstimatedPrice = 222000, Region = "СЗАО" },
            new District { Name = "Строгино"                    ,        EstimatedPrice = 311000, Region = "СЗАО" },
            new District { Name = "Хорошево-Мневники"           ,        EstimatedPrice = 339000, Region = "СЗАО" },
            new District { Name = "Щукино"                      ,        EstimatedPrice = 369000, Region = "СЗАО" },
            new District { Name = "Южное Тушино"                ,        EstimatedPrice = 251000, Region = "СЗАО" },
            
            new District { Name = "Аэропорт"                    ,        EstimatedPrice = 326500, Region = "САО"  },
            new District { Name = "Беговой"                     ,        EstimatedPrice = 311000, Region = "САО"  },
            new District { Name = "Бескудниковский"             ,        EstimatedPrice = 233000, Region = "САО"  },
            new District { Name = "Войковский"                  ,        EstimatedPrice = 333000, Region = "САО"  },
            new District { Name = "Восточное Дегунино"          ,        EstimatedPrice = 198000, Region = "САО"  },
            new District { Name = "Головинский"                 ,        EstimatedPrice = 296000, Region = "САО"  },
            new District { Name = "Дмитровский"                 ,        EstimatedPrice = 224000, Region = "САО"  },
            new District { Name = "Западное Дегунино"           ,        EstimatedPrice = 236000, Region = "САО"  },
            new District { Name = "Коптево"                     ,        EstimatedPrice = 217000, Region = "САО"  },
            new District { Name = "Левобережный"                ,        EstimatedPrice = 262000, Region = "САО"  },
            new District { Name = "Молжаниновский"              ,        EstimatedPrice = 143000, Region = "САО"  },
            new District { Name = "Савеловский"                 ,        EstimatedPrice = 343000, Region = "САО"  },
            new District { Name = "Сокол"                       ,        EstimatedPrice = 285000, Region = "САО"  },
            new District { Name = "Тимирязевский"               ,        EstimatedPrice = 344000, Region = "САО"  },
            new District { Name = "Ховрино"                     ,        EstimatedPrice = 251000, Region = "САО"  },
            new District { Name = "Хорошевский"                 ,        EstimatedPrice = 336000, Region = "САО"  },
            
            new District { Name = "Алексеевский"                ,        EstimatedPrice = 318000, Region = "СВАО" },
            new District { Name = "Алтуфьевский"                ,        EstimatedPrice = 180000, Region = "СВАО" },
            new District { Name = "Бабушкинский"                ,        EstimatedPrice = 232000, Region = "СВАО" },
            new District { Name = "Бибирево"                    ,        EstimatedPrice = 182000, Region = "СВАО" },
            new District { Name = "Бутырский"                   ,        EstimatedPrice = 277000, Region = "СВАО" },
            new District { Name = "Лианозово"                   ,        EstimatedPrice = 241000, Region = "СВАО" },
            new District { Name = "Лосиноостровский"            ,        EstimatedPrice = 244000, Region = "СВАО" },
            new District { Name = "Марфино"                     ,        EstimatedPrice = 253000, Region = "СВАО" },
            new District { Name = "Марьина роща"                ,        EstimatedPrice = 214000, Region = "СВАО" },
            new District { Name = "Останкинский"                ,        EstimatedPrice = 294000, Region = "СВАО" },
            new District { Name = "Отрадное"                    ,        EstimatedPrice = 235000, Region = "СВАО" },
            new District { Name = "Ростокино"                   ,        EstimatedPrice = 181000, Region = "СВАО" },
            new District { Name = "Свиблово"                    ,        EstimatedPrice = 321000, Region = "СВАО" },
            new District { Name = "Северное Медведково"         ,        EstimatedPrice = 249000, Region = "СВАО" },
            new District { Name = "Северный"                    ,        EstimatedPrice = 195000, Region = "СВАО" },
            new District { Name = "Южное Медведково"            ,        EstimatedPrice = 222333, Region = "СВАО" },
            new District { Name = "Ярославский"                 ,        EstimatedPrice = 200000, Region = "СВАО" },
            
            new District { Name = "Вороновское"                 ,        EstimatedPrice = 139000, Region = "ТАО"  },
            new District { Name = "Киевский"                    ,        EstimatedPrice = 109000, Region = "ТАО"  },
            new District { Name = "Клёновское"                  ,        EstimatedPrice = 90000, Region = "ТАО"   },
            new District { Name = "Краснопахорское"             ,        EstimatedPrice = 149000, Region = "ТАО"  },
            new District { Name = "Михайлово-Ярцевское"         ,        EstimatedPrice = 139000, Region = "ТАО"  },
            new District { Name = "Новофёдоровское"             ,        EstimatedPrice = 103000, Region = "ТАО"  },
            new District { Name = "Первомайское"                ,        EstimatedPrice = 112000, Region = "ТАО"  },
            new District { Name = "Роговское"                   ,        EstimatedPrice = 71000, Region = "ТАО"   , ArrowDownCount = 2},
            new District { Name = "Троицк"                      ,        EstimatedPrice = 162000, Region = "ТАО"  , ArrowDownCount = 2},
            new District { Name = "Щаповское"                   ,        EstimatedPrice = 99000, Region = "ТАО"   , ArrowDownCount = 2},
            
            new District { Name = "Внуково"                     ,        EstimatedPrice = 146000, Region = "ЗАО"  },
            new District { Name = "Дорогомилово"                ,        EstimatedPrice = 682000, Region = "ЗАО"  },
            new District { Name = "Крылатское"                  ,        EstimatedPrice = 210213, Region = "ЗАО"  },
            new District { Name = "Кунцево"                     ,        EstimatedPrice = 273000, Region = "ЗАО"  },
            new District { Name = "Можайский"                   ,        EstimatedPrice = 284000, Region = "ЗАО"  },
            new District { Name = "Ново-Переделкино"            ,        EstimatedPrice = 212000, Region = "ЗАО"  },
            new District { Name = "Очаково-Матвеевское"         ,        EstimatedPrice = 293000, Region = "ЗАО"  },
            new District { Name = "Проспект Вернадского"        ,        EstimatedPrice = 293000, Region = "ЗАО"  },
            new District { Name = "Раменки"                     ,        EstimatedPrice = 492000, Region = "ЗАО"  },
            new District { Name = "Солнцево"                    ,        EstimatedPrice = 230000, Region = "ЗАО"  },
            new District { Name = "Тропарево-Никулино"          ,        EstimatedPrice = 375000, Region = "ЗАО"  , ArrowDownCount = 4},
            new District { Name = "Филевский парк"              ,        EstimatedPrice = 327000, Region = "ЗАО"  , ArrowDownCount = 4},
            new District { Name = "Фили-Давыдково"              ,        EstimatedPrice = 311250, Region = "ЗАО"  , ArrowDownCount = 4},
            new District { Name = "Конезавод"                   ,        EstimatedPrice = 220000, Region = "ЗАО"  , ArrowDownCount = 4},
            new District { Name = "Рублёво-Архангельское"       ,        EstimatedPrice = 220000, Region = "ЗАО"  , ArrowDownCount = 4},
            new District { Name = "Сколково"                    ,        EstimatedPrice = 220000, Region = "ЗАО"  , ArrowDownCount = 4},
            
            new District { Name = "Арбат"                       ,        EstimatedPrice = 753000, Region = "ЦАО"  },
            new District { Name = "Басманный"                   ,        EstimatedPrice = 361000, Region = "ЦАО"  },
            new District { Name = "Замоскворечье"               ,        EstimatedPrice = 611000, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Красносельский"              ,        EstimatedPrice = 395000, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Мещанский"                   ,        EstimatedPrice = 777000, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Пресненский"                 ,        EstimatedPrice = 492750, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Таганский"                   ,        EstimatedPrice = 411333, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Тверской"                    ,        EstimatedPrice = 689000, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Хамовники"                   ,        EstimatedPrice = 798250, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Якиманка"                    ,        EstimatedPrice = 420000, Region = "ЦАО"  , ArrowDownCount = 4},
            
            new District { Name = "Богородское"                 ,        EstimatedPrice = 283000, Region = "ВАО"  },
            new District { Name = "Вешняки"                     ,        EstimatedPrice = 184500, Region = "ВАО"  },
            new District { Name = "Восточное Измайлово"         ,        EstimatedPrice = 195000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Восточный"                   ,        EstimatedPrice = 151000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Гольяново"                   ,        EstimatedPrice = 203000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Ивановское"                  ,        EstimatedPrice = 187000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Измайлово"                   ,        EstimatedPrice = 332000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Косино-Ухтомский"            ,        EstimatedPrice = 180000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Метрогородок"                ,        EstimatedPrice = 220000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Новогиреево"                 ,        EstimatedPrice = 206000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Новокосино"                  ,        EstimatedPrice = 179000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Перово"                      ,        EstimatedPrice = 251000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Преображенское"              ,        EstimatedPrice = 363000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Северное Измайлово"          ,        EstimatedPrice = 210000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Соколиная гора"              ,        EstimatedPrice = 273000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Сокольники"                  ,        EstimatedPrice = 244000, Region = "ВАО"  , ArrowDownCount = 5},
            
            new District { Name = "Академический"               ,        EstimatedPrice = 307000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Гагаринский"                 ,        EstimatedPrice = 446333, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Зюзино"                      ,        EstimatedPrice = 270000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Коньково"                    ,        EstimatedPrice = 208000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Котловка"                    ,        EstimatedPrice = 239000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Ломоносовский"               ,        EstimatedPrice = 330000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Обручевский"                 ,        EstimatedPrice = 338000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Северное Бутово"             ,        EstimatedPrice = 207000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Теплый Стан"                 ,        EstimatedPrice = 257000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Черемушки"                   ,        EstimatedPrice = 400000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Южное Бутово"                ,        EstimatedPrice = 208666, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Ясенево"                     ,        EstimatedPrice = 193000, Region = "ЮЗАО" , ArrowDownCount = 14},
            
            new District { Name = "Бирюлево Восточное"          ,        EstimatedPrice = 194000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Бирюлево Западное"           ,        EstimatedPrice = 161000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Братеево"                    ,        EstimatedPrice = 186000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Даниловский"                 ,        EstimatedPrice = 298000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Донской"                     ,        EstimatedPrice = 503000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Зябликово"                   ,        EstimatedPrice = 203000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Москворечье-Сабурово"        ,        EstimatedPrice = 200000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Нагатино-Садовники"          ,        EstimatedPrice = 242000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Нагатинский затон"           ,        EstimatedPrice = 343000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Нагорный"                    ,        EstimatedPrice = 304000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Орехово-Борисово Северное"   ,        EstimatedPrice = 215000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Орехово-Борисово Южное"      ,        EstimatedPrice = 243000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Царицыно"                    ,        EstimatedPrice = 243000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Чертаново Северное"          ,        EstimatedPrice = 226000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Чертаново Центральное"       ,        EstimatedPrice = 199000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Чертаново Южное"             ,        EstimatedPrice = 246000, Region = "ЮАО"  , ArrowDownCount = 14},
            
            new District { Name = "Выхино-Жулебино"             ,        EstimatedPrice = 201000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Капотня"                     ,        EstimatedPrice = 156000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Кузьминки"                   ,        EstimatedPrice = 229000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Лефортово"                   ,        EstimatedPrice = 259666, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Люблино"                     ,        EstimatedPrice = 199000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Марьино"                     ,        EstimatedPrice = 213000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Некрасовка"                  ,        EstimatedPrice = 164800, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Нижегородский"               ,        EstimatedPrice = 264333, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Печатники"                   ,        EstimatedPrice = 188000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Рязанский"                   ,        EstimatedPrice = 231000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Текстильщики"                ,        EstimatedPrice = 217000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Южнопортовый"                ,        EstimatedPrice = 293000, Region = "ЮВАО" , ArrowDownCount = 14},
        };

        private static readonly List<District> _moscowDistrictsAfter1990Before2010 = new List<District>()
        {
            new District { Name = "Внуковское"                  ,        EstimatedPrice = 170000, Region = "НАО"  },
            new District { Name = "Воскресенское"               ,        EstimatedPrice = 178000, Region = "НАО"  },
            new District { Name = "Десёновское"                 ,        EstimatedPrice = 156000, Region = "НАО"  },
            new District { Name = "Кокошкино"                   ,        EstimatedPrice = 135000, Region = "НАО"  },
            new District { Name = "Марушкинское"                ,        EstimatedPrice = 132000, Region = "НАО"  },
            new District { Name = "Московский"                  ,        EstimatedPrice = 180000, Region = "НАО"  },
            new District { Name = "Мосрентген"                  ,        EstimatedPrice = 166000, Region = "НАО"  },
            new District { Name = "Рязановское"                 ,        EstimatedPrice = 150000, Region = "НАО"  },
            new District { Name = "Сосенское"                   ,        EstimatedPrice = 188000, Region = "НАО"  },
            new District { Name = "Филимонковское"              ,        EstimatedPrice = 116000, Region = "НАО"  },
            new District { Name = "Щербинка"                    ,        EstimatedPrice = 146000, Region = "НАО"  },
            
            new District { Name = "Куркино"                     ,        EstimatedPrice = 228000, Region = "СЗАО" },
            new District { Name = "Митино"                      ,        EstimatedPrice = 208500, Region = "СЗАО" },
            new District { Name = "Покровское-Стрешнево"        ,        EstimatedPrice = 261000, Region = "СЗАО" },
            new District { Name = "Северное Тушино"             ,        EstimatedPrice = 238000, Region = "СЗАО" },
            new District { Name = "Строгино"                    ,        EstimatedPrice = 326000, Region = "СЗАО" },
            new District { Name = "Хорошево-Мневники"           ,        EstimatedPrice = 293000, Region = "СЗАО" },
            new District { Name = "Щукино"                      ,        EstimatedPrice = 367000, Region = "СЗАО" },
            new District { Name = "Южное Тушино"                ,        EstimatedPrice = 226000, Region = "СЗАО" },
            
            new District { Name = "Аэропорт"                    ,        EstimatedPrice = 297500, Region = "САО"  },
            new District { Name = "Беговой"                     ,        EstimatedPrice = 358500, Region = "САО"  },
            new District { Name = "Бескудниковский"             ,        EstimatedPrice = 223500, Region = "САО"  },
            new District { Name = "Войковский"                  ,        EstimatedPrice = 254000, Region = "САО"  },
            new District { Name = "Восточное Дегунино"          ,        EstimatedPrice = 213284, Region = "САО"  },
            new District { Name = "Головинский"                 ,        EstimatedPrice = 236000, Region = "САО"  },
            new District { Name = "Дмитровский"                 ,        EstimatedPrice = 207000, Region = "САО"  },
            new District { Name = "Западное Дегунино"           ,        EstimatedPrice = 212000, Region = "САО"  },
            new District { Name = "Коптево"                     ,        EstimatedPrice = 224000, Region = "САО"  },
            new District { Name = "Левобережный"                ,        EstimatedPrice = 240000, Region = "САО"  },
            new District { Name = "Молжаниновский"              ,        EstimatedPrice = 125000, Region = "САО"  },
            new District { Name = "Савеловский"                 ,        EstimatedPrice = 343000, Region = "САО"  },
            new District { Name = "Сокол"                       ,        EstimatedPrice = 370000, Region = "САО"  },
            new District { Name = "Тимирязевский"               ,        EstimatedPrice = 298000, Region = "САО"  },
            new District { Name = "Ховрино"                     ,        EstimatedPrice = 244000, Region = "САО"  },
            new District { Name = "Хорошевский"                 ,        EstimatedPrice = 335000, Region = "САО"  },
            
            new District { Name = "Алексеевский"                ,        EstimatedPrice = 334000, Region = "СВАО" },
            new District { Name = "Алтуфьевский"                ,        EstimatedPrice = 201500, Region = "СВАО" },
            new District { Name = "Бабушкинский"                ,        EstimatedPrice = 276000, Region = "СВАО" },
            new District { Name = "Бибирево"                    ,        EstimatedPrice = 218000, Region = "СВАО" },
            new District { Name = "Бутырский"                   ,        EstimatedPrice = 259000, Region = "СВАО" },
            new District { Name = "Лианозово"                   ,        EstimatedPrice = 215000, Region = "СВАО" },
            new District { Name = "Лосиноостровский"            ,        EstimatedPrice = 208000, Region = "СВАО" },
            new District { Name = "Марфино"                     ,        EstimatedPrice = 291500, Region = "СВАО" },
            new District { Name = "Марьина роща"                ,        EstimatedPrice = 240000, Region = "СВАО" },
            new District { Name = "Останкинский"                ,        EstimatedPrice = 414000, Region = "СВАО" },
            new District { Name = "Отрадное"                    ,        EstimatedPrice = 248000, Region = "СВАО" },
            new District { Name = "Ростокино"                   ,        EstimatedPrice = 275000, Region = "СВАО" },
            new District { Name = "Свиблово"                    ,        EstimatedPrice = 265000, Region = "СВАО" },
            new District { Name = "Северное Медведково"         ,        EstimatedPrice = 232000, Region = "СВАО" },
            new District { Name = "Северный"                    ,        EstimatedPrice = 188000, Region = "СВАО" },
            new District { Name = "Южное Медведково"            ,        EstimatedPrice = 223000, Region = "СВАО" },
            new District { Name = "Ярославский"                 ,        EstimatedPrice = 210000, Region = "СВАО" },
            
            new District { Name = "Вороновское"                 ,        EstimatedPrice = 139000, Region = "ТАО"  },
            new District { Name = "Киевский"                    ,        EstimatedPrice = 109000, Region = "ТАО"  },
            new District { Name = "Клёновское"                  ,        EstimatedPrice = 90000, Region = "ТАО"   },
            new District { Name = "Краснопахорское"             ,        EstimatedPrice = 149000, Region = "ТАО"  },
            new District { Name = "Михайлово-Ярцевское"         ,        EstimatedPrice = 126000, Region = "ТАО"  },
            new District { Name = "Новофёдоровское"             ,        EstimatedPrice = 103000, Region = "ТАО"  },
            new District { Name = "Первомайское"                ,        EstimatedPrice = 112000, Region = "ТАО"  },
            new District { Name = "Роговское"                   ,        EstimatedPrice = 71000, Region = "ТАО"   , ArrowDownCount = 2},
            new District { Name = "Троицк"                      ,        EstimatedPrice = 162000, Region = "ТАО"  , ArrowDownCount = 2},
            new District { Name = "Щаповское"                   ,        EstimatedPrice = 99000, Region = "ТАО"   , ArrowDownCount = 2},
            
            new District { Name = "Внуково"                     ,        EstimatedPrice = 150000, Region = "ЗАО"  },
            new District { Name = "Дорогомилово"                ,        EstimatedPrice = 448000, Region = "ЗАО"  },
            new District { Name = "Крылатское"                  ,        EstimatedPrice = 357000, Region = "ЗАО"  },
            new District { Name = "Кунцево"                     ,        EstimatedPrice = 288000, Region = "ЗАО"  },
            new District { Name = "Можайский"                   ,        EstimatedPrice = 224000, Region = "ЗАО"  },
            new District { Name = "Ново-Переделкино"            ,        EstimatedPrice = 198000, Region = "ЗАО"  },
            new District { Name = "Очаково-Матвеевское"         ,        EstimatedPrice = 379000, Region = "ЗАО"  },
            new District { Name = "Проспект Вернадского"        ,        EstimatedPrice = 373000, Region = "ЗАО"  },
            new District { Name = "Раменки"                     ,        EstimatedPrice = 414000, Region = "ЗАО"  },
            new District { Name = "Солнцево"                    ,        EstimatedPrice = 212000, Region = "ЗАО"  },
            new District { Name = "Тропарево-Никулино"          ,        EstimatedPrice = 338000, Region = "ЗАО"  , ArrowDownCount = 4},
            new District { Name = "Филевский парк"              ,        EstimatedPrice = 288000, Region = "ЗАО"  , ArrowDownCount = 4},
            new District { Name = "Фили-Давыдково"              ,        EstimatedPrice = 327000, Region = "ЗАО"  , ArrowDownCount = 4},
            new District { Name = "Конезавод"                   ,        EstimatedPrice = 220000, Region = "ЗАО"  , ArrowDownCount = 4},
            new District { Name = "Рублёво-Архангельское"       ,        EstimatedPrice = 220000, Region = "ЗАО"  , ArrowDownCount = 4},
            new District { Name = "Сколково"                    ,        EstimatedPrice = 220000, Region = "ЗАО"  , ArrowDownCount = 4},
            
            new District { Name = "Арбат"                       ,        EstimatedPrice = 708000, Region = "ЦАО"  },
            new District { Name = "Басманный"                   ,        EstimatedPrice = 389000, Region = "ЦАО"  },
            new District { Name = "Замоскворечье"               ,        EstimatedPrice = 463000, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Красносельский"              ,        EstimatedPrice = 336000, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Мещанский"                   ,        EstimatedPrice = 483750, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Пресненский"                 ,        EstimatedPrice = 500666, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Таганский"                   ,        EstimatedPrice = 363000, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Тверской"                    ,        EstimatedPrice = 564000, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Хамовники"                   ,        EstimatedPrice = 809333, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Якиманка"                    ,        EstimatedPrice = 500000, Region = "ЦАО"  , ArrowDownCount = 4},
            
            new District { Name = "Богородское"                 ,        EstimatedPrice = 231000, Region = "ВАО"  },
            new District { Name = "Вешняки"                     ,        EstimatedPrice = 183000, Region = "ВАО"  },
            new District { Name = "Восточное Измайлово"         ,        EstimatedPrice = 242500, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Восточный"                   ,        EstimatedPrice = 151000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Гольяново"                   ,        EstimatedPrice = 207000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Ивановское"                  ,        EstimatedPrice = 214000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Измайлово"                   ,        EstimatedPrice = 283000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Косино-Ухтомский"            ,        EstimatedPrice = 197000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Метрогородок"                ,        EstimatedPrice = 201000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Новогиреево"                 ,        EstimatedPrice = 250000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Новокосино"                  ,        EstimatedPrice = 196000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Перово"                      ,        EstimatedPrice = 232000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Преображенское"              ,        EstimatedPrice = 300000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Северное Измайлово"          ,        EstimatedPrice = 227000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Соколиная гора"              ,        EstimatedPrice = 254000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Сокольники"                  ,        EstimatedPrice = 244000, Region = "ВАО"  , ArrowDownCount = 5},
            
            new District { Name = "Академический"               ,        EstimatedPrice = 273333, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Гагаринский"                 ,        EstimatedPrice = 482000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Зюзино"                      ,        EstimatedPrice = 253000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Коньково"                    ,        EstimatedPrice = 281000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Котловка"                    ,        EstimatedPrice = 291000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Ломоносовский"               ,        EstimatedPrice = 336000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Обручевский"                 ,        EstimatedPrice = 286000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Северное Бутово"             ,        EstimatedPrice = 221830, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Теплый Стан"                 ,        EstimatedPrice = 286000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Черемушки"                   ,        EstimatedPrice = 301000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Южное Бутово"                ,        EstimatedPrice = 194333, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Ясенево"                     ,        EstimatedPrice = 193000, Region = "ЮЗАО" , ArrowDownCount = 14},
            
            new District { Name = "Бирюлево Восточное"          ,        EstimatedPrice = 166000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Бирюлево Западное"           ,        EstimatedPrice = 212500, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Братеево"                    ,        EstimatedPrice = 192000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Даниловский"                 ,        EstimatedPrice = 291000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Донской"                     ,        EstimatedPrice = 254000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Зябликово"                   ,        EstimatedPrice = 203000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Москворечье-Сабурово"        ,        EstimatedPrice = 217000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Нагатино-Садовники"          ,        EstimatedPrice = 259000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Нагатинский затон"           ,        EstimatedPrice = 300400, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Нагорный"                    ,        EstimatedPrice = 262000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Орехово-Борисово Северное"   ,        EstimatedPrice = 211000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Орехово-Борисово Южное"      ,        EstimatedPrice = 200000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Царицыно"                    ,        EstimatedPrice = 216000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Чертаново Северное"          ,        EstimatedPrice = 268000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Чертаново Центральное"       ,        EstimatedPrice = 247000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Чертаново Южное"             ,        EstimatedPrice = 213000, Region = "ЮАО"  , ArrowDownCount = 14},
            
            new District { Name = "Выхино-Жулебино"             ,        EstimatedPrice = 198666, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Капотня"                     ,        EstimatedPrice = 176000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Кузьминки"                   ,        EstimatedPrice = 233800, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Лефортово"                   ,        EstimatedPrice = 264000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Люблино"                     ,        EstimatedPrice = 209000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Марьино"                     ,        EstimatedPrice = 212000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Некрасовка"                  ,        EstimatedPrice = 157500, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Нижегородский"               ,        EstimatedPrice = 231000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Печатники"                   ,        EstimatedPrice = 201000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Рязанский"                   ,        EstimatedPrice = 231000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Текстильщики"                ,        EstimatedPrice = 171000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Южнопортовый"                ,        EstimatedPrice = 256000, Region = "ЮВАО" , ArrowDownCount = 14},
        };

        private static readonly List<District> _moscowDistrictsAllTimes = new List<District>()
        {
            new District { Name = "Внуковское"                  ,        EstimatedPrice = 198500, Region = "НАО"  },
            new District { Name = "Воскресенское"               ,        EstimatedPrice = 172000, Region = "НАО"  },
            new District { Name = "Десёновское"                 ,        EstimatedPrice = 161500, Region = "НАО"  },
            new District { Name = "Кокошкино"                   ,        EstimatedPrice = 148333, Region = "НАО"  },
            new District { Name = "Марушкинское"                ,        EstimatedPrice = 128000, Region = "НАО"  },
            new District { Name = "Московский"                  ,        EstimatedPrice = 192750, Region = "НАО"  },
            new District { Name = "Мосрентген"                  ,        EstimatedPrice = 186666, Region = "НАО"  },
            new District { Name = "Рязановское"                 ,        EstimatedPrice = 152666, Region = "НАО"  },
            new District { Name = "Сосенское"                   ,        EstimatedPrice = 184830, Region = "НАО"  },
            new District { Name = "Филимонковское"              ,        EstimatedPrice = 121000, Region = "НАО"  },
            new District { Name = "Щербинка"                    ,        EstimatedPrice = 155000, Region = "НАО"  },

            new District { Name = "Куркино"                     ,        EstimatedPrice = 232000, Region = "СЗАО" },
            new District { Name = "Митино"                      ,        EstimatedPrice = 207000, Region = "СЗАО" },
            new District { Name = "Покровское-Стрешнево"        ,        EstimatedPrice = 250500, Region = "СЗАО" },
            new District { Name = "Северное Тушино"             ,        EstimatedPrice = 218000, Region = "СЗАО" },
            new District { Name = "Строгино"                    ,        EstimatedPrice = 246500, Region = "СЗАО" },
            new District { Name = "Хорошево-Мневники"           ,        EstimatedPrice = 314000, Region = "СЗАО" },
            new District { Name = "Щукино"                      ,        EstimatedPrice = 282000, Region = "СЗАО" },
            new District { Name = "Южное Тушино"                ,        EstimatedPrice = 214000, Region = "СЗАО" },

            new District { Name = "Аэропорт"                    ,        EstimatedPrice = 281666, Region = "САО"  },
            new District { Name = "Беговой"                     ,        EstimatedPrice = 323000, Region = "САО"  },
            new District { Name = "Бескудниковский"             ,        EstimatedPrice = 219666, Region = "САО"  },
            new District { Name = "Войковский"                  ,        EstimatedPrice = 293333, Region = "САО"  },
            new District { Name = "Восточное Дегунино"          ,        EstimatedPrice = 213426, Region = "САО"  },
            new District { Name = "Головинский"                 ,        EstimatedPrice = 234500, Region = "САО"  },
            new District { Name = "Дмитровский"                 ,        EstimatedPrice = 207666, Region = "САО"  },
            new District { Name = "Западное Дегунино"           ,        EstimatedPrice = 212000, Region = "САО"  },
            new District { Name = "Коптево"                     ,        EstimatedPrice = 217000, Region = "САО"  },
            new District { Name = "Левобережный"                ,        EstimatedPrice = 240000, Region = "САО"  },
            new District { Name = "Молжаниновский"              ,        EstimatedPrice = 143000, Region = "САО"  },
            new District { Name = "Савеловский"                 ,        EstimatedPrice = 260000, Region = "САО"  },
            new District { Name = "Сокол"                       ,        EstimatedPrice = 277500, Region = "САО"  },
            new District { Name = "Тимирязевский"               ,        EstimatedPrice = 242000, Region = "САО"  },
            new District { Name = "Ховрино"                     ,        EstimatedPrice = 233000, Region = "САО"  },
            new District { Name = "Хорошевский"                 ,        EstimatedPrice = 334333, Region = "САО"  },

            new District { Name = "Алексеевский"                ,        EstimatedPrice = 277000, Region = "СВАО" },
            new District { Name = "Алтуфьевский"                ,        EstimatedPrice = 190000, Region = "СВАО" },
            new District { Name = "Бабушкинский"                ,        EstimatedPrice = 222000, Region = "СВАО" },
            new District { Name = "Бибирево"                    ,        EstimatedPrice = 200000, Region = "СВАО" },
            new District { Name = "Бутырский"                   ,        EstimatedPrice = 255000, Region = "СВАО" },
            new District { Name = "Лианозово"                   ,        EstimatedPrice = 199000, Region = "СВАО" },
            new District { Name = "Лосиноостровский"            ,        EstimatedPrice = 195500, Region = "СВАО" },
            new District { Name = "Марфино"                     ,        EstimatedPrice = 246000, Region = "СВАО" },
            new District { Name = "Марьина роща"                ,        EstimatedPrice = 237000, Region = "СВАО" },
            new District { Name = "Останкинский"                ,        EstimatedPrice = 263000, Region = "СВАО" },
            new District { Name = "Отрадное"                    ,        EstimatedPrice = 218500, Region = "СВАО" },
            new District { Name = "Ростокино"                   ,        EstimatedPrice = 194000, Region = "СВАО" },
            new District { Name = "Свиблово"                    ,        EstimatedPrice = 242000, Region = "СВАО" },
            new District { Name = "Северное Медведково"         ,        EstimatedPrice = 223000, Region = "СВАО" },
            new District { Name = "Северный"                    ,        EstimatedPrice = 193000, Region = "СВАО" },
            new District { Name = "Южное Медведково"            ,        EstimatedPrice = 215000, Region = "СВАО" },
            new District { Name = "Ярославский"                 ,        EstimatedPrice = 192000, Region = "СВАО" },

            new District { Name = "Вороновское"                 ,        EstimatedPrice = 139000,  Region = "ТАО"  },
            new District { Name = "Киевский"                    ,        EstimatedPrice = 109000,  Region = "ТАО"  },
            new District { Name = "Клёновское"                  ,        EstimatedPrice = 90000 ,  Region = "ТАО"  },
            new District { Name = "Краснопахорское"             ,        EstimatedPrice = 124000,  Region = "ТАО"  },
            new District { Name = "Михайлово-Ярцевское"         ,        EstimatedPrice = 138000,  Region = "ТАО"  },
            new District { Name = "Новофёдоровское"             ,        EstimatedPrice = 99000 ,  Region = "ТАО"  },
            new District { Name = "Первомайское"                ,        EstimatedPrice = 132000,  Region = "ТАО"  },
            new District { Name = "Роговское"                   ,        EstimatedPrice = 71000 ,  Region = "ТАО"  , ArrowDownCount = 2},
            new District { Name = "Троицк"                      ,        EstimatedPrice = 161500,  Region = "ТАО"  , ArrowDownCount = 2},
            new District { Name = "Щаповское"                   ,        EstimatedPrice = 121000,  Region = "ТАО"  , ArrowDownCount = 2},

            new District { Name = "Внуково"                     ,        EstimatedPrice = 150000, Region = "ЗАО"  },
            new District { Name = "Дорогомилово"                ,        EstimatedPrice = 372000, Region = "ЗАО"  },
            new District { Name = "Крылатское"                  ,        EstimatedPrice = 239000, Region = "ЗАО"  },
            new District { Name = "Кунцево"                     ,        EstimatedPrice = 262000, Region = "ЗАО"  },
            new District { Name = "Можайский"                   ,        EstimatedPrice = 224250, Region = "ЗАО"  },
            new District { Name = "Ново-Переделкино"            ,        EstimatedPrice = 203000, Region = "ЗАО"  },
            new District { Name = "Очаково-Матвеевское"         ,        EstimatedPrice = 266000, Region = "ЗАО"  },
            new District { Name = "Проспект Вернадского"        ,        EstimatedPrice = 305000, Region = "ЗАО"  },
            new District { Name = "Раменки"                     ,        EstimatedPrice = 427000, Region = "ЗАО"  },
            new District { Name = "Солнцево"                    ,        EstimatedPrice = 222000, Region = "ЗАО"  },
            new District { Name = "Тропарево-Никулино"          ,        EstimatedPrice = 322000, Region = "ЗАО"  , ArrowDownCount = 4},
            new District { Name = "Филевский парк"              ,        EstimatedPrice = 285000, Region = "ЗАО"  , ArrowDownCount = 4},
            new District { Name = "Фили-Давыдково"              ,        EstimatedPrice = 278000, Region = "ЗАО"  , ArrowDownCount = 4},
            new District { Name = "Конезавод"                   ,        EstimatedPrice = 220000, Region = "ЗАО"  , ArrowDownCount = 4},
            new District { Name = "Рублёво-Архангельское"       ,        EstimatedPrice = 220000, Region = "ЗАО"  , ArrowDownCount = 4},
            new District { Name = "Сколково"                    ,        EstimatedPrice = 220000, Region = "ЗАО"  , ArrowDownCount = 4},

            new District { Name = "Арбат"                       ,        EstimatedPrice = 644500, Region = "ЦАО"  },
            new District { Name = "Басманный"                   ,        EstimatedPrice = 356000, Region = "ЦАО"  },
            new District { Name = "Замоскворечье"               ,        EstimatedPrice = 450000, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Красносельский"              ,        EstimatedPrice = 327500, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Мещанский"                   ,        EstimatedPrice = 310000, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Пресненский"                 ,        EstimatedPrice = 460830, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Таганский"                   ,        EstimatedPrice = 349000, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Тверской"                    ,        EstimatedPrice = 519000, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Хамовники"                   ,        EstimatedPrice = 620550, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Якиманка"                    ,        EstimatedPrice = 466333, Region = "ЦАО"  , ArrowDownCount = 6},

            new District { Name = "Богородское"                 ,        EstimatedPrice = 236000, Region = "ВАО"  },
            new District { Name = "Вешняки"                     ,        EstimatedPrice = 184000, Region = "ВАО"  },
            new District { Name = "Восточное Измайлово"         ,        EstimatedPrice = 213000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Восточный"                   ,        EstimatedPrice = 164000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Гольяново"                   ,        EstimatedPrice = 202750, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Ивановское"                  ,        EstimatedPrice = 188000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Измайлово"                   ,        EstimatedPrice = 216900, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Косино-Ухтомский"            ,        EstimatedPrice = 187000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Метрогородок"                ,        EstimatedPrice = 200333, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Новогиреево"                 ,        EstimatedPrice = 207000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Новокосино"                  ,        EstimatedPrice = 194000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Перово"                      ,        EstimatedPrice = 209000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Преображенское"              ,        EstimatedPrice = 264000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Северное Измайлово"          ,        EstimatedPrice = 204666, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Соколиная гора"              ,        EstimatedPrice = 235000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Сокольники"                  ,        EstimatedPrice = 255000, Region = "ВАО"  , ArrowDownCount = 5},

            new District { Name = "Академический"               ,        EstimatedPrice = 285000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Гагаринский"                 ,        EstimatedPrice = 311000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Зюзино"                      ,        EstimatedPrice = 236666, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Коньково"                    ,        EstimatedPrice = 235000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Котловка"                    ,        EstimatedPrice = 246000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Ломоносовский"               ,        EstimatedPrice = 289333, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Обручевский"                 ,        EstimatedPrice = 321000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Северное Бутово"             ,        EstimatedPrice = 210000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Теплый Стан"                 ,        EstimatedPrice = 237500, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Черемушки"                   ,        EstimatedPrice = 269000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Южное Бутово"                ,        EstimatedPrice = 201333, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Ясенево"                     ,        EstimatedPrice = 206000, Region = "ЮЗАО" , ArrowDownCount = 14},

            new District { Name = "Бирюлево Восточное"          ,        EstimatedPrice = 180000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Бирюлево Западное"           ,        EstimatedPrice = 172000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Братеево"                    ,        EstimatedPrice = 201000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Даниловский"                 ,        EstimatedPrice = 280000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Донской"                     ,        EstimatedPrice = 293000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Зябликово"                   ,        EstimatedPrice = 195000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Москворечье-Сабурово"        ,        EstimatedPrice = 208666, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Нагатино-Садовники"          ,        EstimatedPrice = 234000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Нагатинский затон"           ,        EstimatedPrice = 260000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Нагорный"                    ,        EstimatedPrice = 231000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Орехово-Борисово Северное"   ,        EstimatedPrice = 203000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Орехово-Борисово Южное"      ,        EstimatedPrice = 207000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Царицыно"                    ,        EstimatedPrice = 209000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Чертаново Северное"          ,        EstimatedPrice = 241000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Чертаново Центральное"       ,        EstimatedPrice = 219500, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Чертаново Южное"             ,        EstimatedPrice = 214500, Region = "ЮАО"  , ArrowDownCount = 14},

            new District { Name = "Выхино-Жулебино"             ,        EstimatedPrice = 194800, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Капотня"                     ,        EstimatedPrice = 175500, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Кузьминки"                   ,        EstimatedPrice = 203000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Лефортово"                   ,        EstimatedPrice = 257000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Люблино"                     ,        EstimatedPrice = 203400, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Марьино"                     ,        EstimatedPrice = 208750, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Некрасовка"                  ,        EstimatedPrice = 161500, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Нижегородский"               ,        EstimatedPrice = 233000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Печатники"                   ,        EstimatedPrice = 202500, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Рязанский"                   ,        EstimatedPrice = 212750, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Текстильщики"                ,        EstimatedPrice = 196000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Южнопортовый"                ,        EstimatedPrice = 246500, Region = "ЮВАО" , ArrowDownCount = 14},
        };

        private static readonly List<District> _moscowDistricts = new List<District>()
        {
            new District { Name = "Внуковское"                  ,        EstimatedPrice = 170000, Region = "НАО"  },
            new District { Name = "Воскресенское"               ,        EstimatedPrice = 142000, Region = "НАО"  },
            new District { Name = "Десёновское"                 ,        EstimatedPrice = 142000, Region = "НАО"  },
            new District { Name = "Кокошкино"                   ,        EstimatedPrice = 135000, Region = "НАО"  },
            new District { Name = "Марушкинское"                ,        EstimatedPrice = 132000, Region = "НАО"  },
            new District { Name = "Московский"                  ,        EstimatedPrice = 154000, Region = "НАО"  },
            new District { Name = "Мосрентген"                  ,        EstimatedPrice = 166000, Region = "НАО"  },
            new District { Name = "Рязановское"                 ,        EstimatedPrice = 138000, Region = "НАО"  },
            new District { Name = "Сосенское"                   ,        EstimatedPrice = 160000, Region = "НАО"  },
            new District { Name = "Филимонковское"              ,        EstimatedPrice = 116000, Region = "НАО"  },
            new District { Name = "Щербинка"                    ,        EstimatedPrice = 140000, Region = "НАО"  },

            new District { Name = "Куркино"                     ,        EstimatedPrice = 220000, Region = "СЗАО" },
            new District { Name = "Митино"                      ,        EstimatedPrice = 190000, Region = "СЗАО" },
            new District { Name = "Покровское-Стрешнево"        ,        EstimatedPrice = 204000, Region = "СЗАО" },
            new District { Name = "Северное Тушино"             ,        EstimatedPrice = 203000, Region = "СЗАО" },
            new District { Name = "Строгино"                    ,        EstimatedPrice = 237000, Region = "СЗАО" },
            new District { Name = "Хорошево-Мневники"           ,        EstimatedPrice = 251000, Region = "СЗАО" },
            new District { Name = "Щукино"                      ,        EstimatedPrice = 294000, Region = "СЗАО" },
            new District { Name = "Южное Тушино"                ,        EstimatedPrice = 206000, Region = "СЗАО" },

            new District { Name = "Аэропорт"                    ,        EstimatedPrice = 298000, Region = "САО"  },
            new District { Name = "Беговой"                     ,        EstimatedPrice = 287000, Region = "САО"  },
            new District { Name = "Бескудниковский"             ,        EstimatedPrice = 209000, Region = "САО"  },
            new District { Name = "Войковский"                  ,        EstimatedPrice = 254000, Region = "САО"  },
            new District { Name = "Восточное Дегунино"          ,        EstimatedPrice = 198000, Region = "САО"  },
            new District { Name = "Головинский"                 ,        EstimatedPrice = 236000, Region = "САО"  },
            new District { Name = "Дмитровский"                 ,        EstimatedPrice = 181000, Region = "САО"  },
            new District { Name = "Западное Дегунино"           ,        EstimatedPrice = 198000, Region = "САО"  },
            new District { Name = "Коптево"                     ,        EstimatedPrice = 217000, Region = "САО"  },
            new District { Name = "Левобережный"                ,        EstimatedPrice = 255000, Region = "САО"  },
            new District { Name = "Молжаниновский"              ,        EstimatedPrice = 125000, Region = "САО"  },
            new District { Name = "Савеловский"                 ,        EstimatedPrice = 254000, Region = "САО"  },
            new District { Name = "Сокол"                       ,        EstimatedPrice = 285000, Region = "САО"  },
            new District { Name = "Тимирязевский"               ,        EstimatedPrice = 258000, Region = "САО"  },
            new District { Name = "Ховрино"                     ,        EstimatedPrice = 234000, Region = "САО"  },
            new District { Name = "Хорошевский"                 ,        EstimatedPrice = 313000, Region = "САО"  },

            new District { Name = "Алексеевский"                ,        EstimatedPrice = 272000, Region = "СВАО" },
            new District { Name = "Алтуфьевский"                ,        EstimatedPrice = 179000, Region = "СВАО" },
            new District { Name = "Бабушкинский"                ,        EstimatedPrice = 207000, Region = "СВАО" },
            new District { Name = "Бибирево"                    ,        EstimatedPrice = 182000, Region = "СВАО" },
            new District { Name = "Бутырский"                   ,        EstimatedPrice = 235000, Region = "СВАО" },
            new District { Name = "Лианозово"                   ,        EstimatedPrice = 141000, Region = "СВАО" },
            new District { Name = "Лосиноостровский"            ,        EstimatedPrice = 152000, Region = "СВАО" },
            new District { Name = "Марфино"                     ,        EstimatedPrice = 253000, Region = "СВАО" },
            new District { Name = "Марьина роща"                ,        EstimatedPrice = 219000, Region = "СВАО" },
            new District { Name = "Останкинский"                ,        EstimatedPrice = 206000, Region = "СВАО" },
            new District { Name = "Отрадное"                    ,        EstimatedPrice = 197000, Region = "СВАО" },
            new District { Name = "Ростокино"                   ,        EstimatedPrice = 212000, Region = "СВАО" },
            new District { Name = "Свиблово"                    ,        EstimatedPrice = 220000, Region = "СВАО" },
            new District { Name = "Северное Медведково"         ,        EstimatedPrice = 208000, Region = "СВАО" },
            new District { Name = "Северный"                    ,        EstimatedPrice = 173000, Region = "СВАО" },
            new District { Name = "Южное Медведково"            ,        EstimatedPrice = 194000, Region = "СВАО" },
            new District { Name = "Ярославский"                 ,        EstimatedPrice = 200000, Region = "СВАО" },

            new District { Name = "Вороновское"                 ,        EstimatedPrice = 139000, Region = "ТАО"  },
            new District { Name = "Киевский"                    ,        EstimatedPrice = 109000, Region = "ТАО"  },
            new District { Name = "Клёновское"                  ,        EstimatedPrice = 90000, Region = "ТАО"   },
            new District { Name = "Краснопахорское"             ,        EstimatedPrice = 149000, Region = "ТАО"  },
            new District { Name = "Михайлово-Ярцевское"         ,        EstimatedPrice = 126000, Region = "ТАО"  },
            new District { Name = "Новофёдоровское"             ,        EstimatedPrice = 103000, Region = "ТАО"  },
            new District { Name = "Первомайское"                ,        EstimatedPrice = 112000, Region = "ТАО"  },
            new District { Name = "Роговское"                   ,        EstimatedPrice = 71000, Region = "ТАО"   , ArrowDownCount = 2},
            new District { Name = "Троицк"                      ,        EstimatedPrice = 151000, Region = "ТАО"  , ArrowDownCount = 2},
            new District { Name = "Щаповское"                   ,        EstimatedPrice = 99000, Region = "ТАО"   , ArrowDownCount = 2},

            new District { Name = "Внуково"                     ,        EstimatedPrice = 146000, Region = "ЗАО"  },
            new District { Name = "Дорогомилово"                ,        EstimatedPrice = 446000, Region = "ЗАО"  },
            new District { Name = "Крылатское"                  ,        EstimatedPrice = 234000, Region = "ЗАО"  },
            new District { Name = "Кунцево"                     ,        EstimatedPrice = 232000, Region = "ЗАО"  },
            new District { Name = "Можайский"                   ,        EstimatedPrice = 221000, Region = "ЗАО"  },
            new District { Name = "Ново-Переделкино"            ,        EstimatedPrice = 193000, Region = "ЗАО"  },
            new District { Name = "Очаково-Матвеевское"         ,        EstimatedPrice = 258000, Region = "ЗАО"  },
            new District { Name = "Проспект Вернадского"        ,        EstimatedPrice = 294000, Region = "ЗАО"  },
            new District { Name = "Раменки"                     ,        EstimatedPrice = 326000, Region = "ЗАО"  },
            new District { Name = "Солнцево"                    ,        EstimatedPrice = 205000, Region = "ЗАО"  },
            new District { Name = "Тропарево-Никулино"          ,        EstimatedPrice = 301000, Region = "ЗАО"  , ArrowDownCount = 4},
            new District { Name = "Филевский парк"              ,        EstimatedPrice = 300000, Region = "ЗАО"  , ArrowDownCount = 4},
            new District { Name = "Фили-Давыдково"              ,        EstimatedPrice = 264000, Region = "ЗАО"  , ArrowDownCount = 4},
            new District { Name = "Конезавод"                   ,        EstimatedPrice = 220000, Region = "ЗАО"  , ArrowDownCount = 4},
            new District { Name = "Рублёво-Архангельское"       ,        EstimatedPrice = 220000, Region = "ЗАО"  , ArrowDownCount = 4},
            new District { Name = "Сколково"                    ,        EstimatedPrice = 220000, Region = "ЗАО"  , ArrowDownCount = 4},

            new District { Name = "Арбат"                       ,        EstimatedPrice = 708000, Region = "ЦАО"  },
            new District { Name = "Басманный"                   ,        EstimatedPrice = 321000, Region = "ЦАО"  },
            new District { Name = "Замоскворечье"               ,        EstimatedPrice = 400000, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Красносельский"              ,        EstimatedPrice = 369000, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Мещанский"                   ,        EstimatedPrice = 344000, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Пресненский"                 ,        EstimatedPrice = 441000, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Таганский"                   ,        EstimatedPrice = 354000, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Тверской"                    ,        EstimatedPrice = 510000, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Хамовники"                   ,        EstimatedPrice = 630000, Region = "ЦАО"  , ArrowDownCount = 4},
            new District { Name = "Якиманка"                    ,        EstimatedPrice = 701000, Region = "ЦАО"  , ArrowDownCount = 6},

            new District { Name = "Богородское"                 ,        EstimatedPrice = 230000, Region = "ВАО"  },
            new District { Name = "Вешняки"                     ,        EstimatedPrice = 183000, Region = "ВАО"  },
            new District { Name = "Восточное Измайлово"         ,        EstimatedPrice = 195000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Восточный"                   ,        EstimatedPrice = 151000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Гольяново"                   ,        EstimatedPrice = 185000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Ивановское"                  ,        EstimatedPrice = 187000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Измайлово"                   ,        EstimatedPrice = 215000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Косино-Ухтомский"            ,        EstimatedPrice = 180000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Метрогородок"                ,        EstimatedPrice = 201000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Новогиреево"                 ,        EstimatedPrice = 198000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Новокосино"                  ,        EstimatedPrice = 179000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Перово"                      ,        EstimatedPrice = 208000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Преображенское"              ,        EstimatedPrice = 236000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Северное Измайлово"          ,        EstimatedPrice = 190000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Соколиная гора"              ,        EstimatedPrice = 221000, Region = "ВАО"  , ArrowDownCount = 5},
            new District { Name = "Сокольники"                  ,        EstimatedPrice = 244000, Region = "ВАО"  , ArrowDownCount = 5},

            new District { Name = "Академический"               ,        EstimatedPrice = 288000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Гагаринский"                 ,        EstimatedPrice = 313000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Зюзино"                      ,        EstimatedPrice = 221000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Коньково"                    ,        EstimatedPrice = 232000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Котловка"                    ,        EstimatedPrice = 233000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Ломоносовский"               ,        EstimatedPrice = 290000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Обручевский"                 ,        EstimatedPrice = 284000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Северное Бутово"             ,        EstimatedPrice = 200000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Теплый Стан"                 ,        EstimatedPrice = 221000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Черемушки"                   ,        EstimatedPrice = 316000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Южное Бутово"                ,        EstimatedPrice = 175000, Region = "ЮЗАО" , ArrowDownCount = 14},
            new District { Name = "Ясенево"                     ,        EstimatedPrice = 193000, Region = "ЮЗАО" , ArrowDownCount = 14},

            new District { Name = "Бирюлево Восточное"          ,        EstimatedPrice = 154000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Бирюлево Западное"           ,        EstimatedPrice = 161000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Братеево"                    ,        EstimatedPrice = 186000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Даниловский"                 ,        EstimatedPrice = 300000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Донской"                     ,        EstimatedPrice = 254000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Зябликово"                   ,        EstimatedPrice = 203000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Москворечье-Сабурово"        ,        EstimatedPrice = 200000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Нагатино-Садовники"          ,        EstimatedPrice = 217000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Нагатинский затон"           ,        EstimatedPrice = 251000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Нагорный"                    ,        EstimatedPrice = 259000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Орехово-Борисово Северное"   ,        EstimatedPrice = 199000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Орехово-Борисово Южное"      ,        EstimatedPrice = 203000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Царицыно"                    ,        EstimatedPrice = 195000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Чертаново Северное"          ,        EstimatedPrice = 226000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Чертаново Центральное"       ,        EstimatedPrice = 199000, Region = "ЮАО"  , ArrowDownCount = 14},
            new District { Name = "Чертаново Южное"             ,        EstimatedPrice = 204000, Region = "ЮАО"  , ArrowDownCount = 14},

            new District { Name = "Выхино-Жулебино"             ,        EstimatedPrice = 185000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Капотня"                     ,        EstimatedPrice = 156000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Кузьминки"                   ,        EstimatedPrice = 194000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Лефортово"                   ,        EstimatedPrice = 232000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Люблино"                     ,        EstimatedPrice = 161000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Марьино"                     ,        EstimatedPrice = 190000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Некрасовка"                  ,        EstimatedPrice = 147000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Нижегородский"               ,        EstimatedPrice = 206000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Печатники"                   ,        EstimatedPrice = 188000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Рязанский"                   ,        EstimatedPrice = 226000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Текстильщики"                ,        EstimatedPrice = 166000, Region = "ЮВАО" , ArrowDownCount = 14},
            new District { Name = "Южнопортовый"                ,        EstimatedPrice = 229000, Region = "ЮВАО" , ArrowDownCount = 14},
        };

        static void Main(string[] args)
        {
            var driver = new ChromeDriver(new ChromeOptions() { });
            driver.Manage().Window.Maximize();

            var urlDescriptionMap = new UrlDescriptionMap
            {
                Districts = _moscowDistricts,
                UrlDescriptions = new List<UrlDescription>
                {
                    new UrlDescription
                    {
                        Description = "Москва, 2 комнатные, вторички, после 2010 года, не первый, не последний этажи, от 10 этажей",
                        Url = "https://www.cian.ru/cat.php?deal_type=sale&engine_version=2&floornl=1&is_first_floor=0&m2=1&min_house_year=2010&minfloorn=10&object_type%5B0%5D=1&offer_type=flat&region=1&room2=1",
                        Districts = _MoscowDistrictsAfter2010
                    },

                    new UrlDescription
                    {
                        Description = "Москва, 2 комнатные, вторички, c 1990 до 2010 года, не первый, не последний этажи, от 10 этажей",
                        Url = "https://www.cian.ru/cat.php?deal_type=sale&engine_version=2&floornl=1&is_first_floor=0&m2=1&max_house_year=2010&min_house_year=1990&minfloorn=10&object_type%5B0%5D=1&offer_type=flat&region=1&room2=1",
                        Districts = _moscowDistrictsAfter1990Before2010
                    },

                    new UrlDescription
                    {
                        Description = "Москва, 2 комнатные, вторички",
                        Url = "https://www.cian.ru/cat.php?deal_type=sale&engine_version=2&m2=1&object_type%5B0%5D=1&offer_type=flat&region=1&room2=1",
                        Districts = _moscowDistrictsAllTimes
                    },
                }
            };

            //var urlDescriptionMap = new UrlDescriptionMap
            //{
            //    Districts = new List<District> 
            //    {
            //         new District { Name = "Железнодорожный"             ,        EstimatedPrice = 54000, Region = "Орел" , ArrowDownCount = 0},
            //         new District { Name = "Заводской"             ,        EstimatedPrice = 58000, Region = "Орел" , ArrowDownCount = 0},
            //         new District { Name = "Северный"             ,        EstimatedPrice = 48000, Region = "Орел" , ArrowDownCount = 0},
            //         new District { Name = "Советский"             ,        EstimatedPrice = 57000, Region = "Орел" , ArrowDownCount = 0},
            //    },
            //    UrlDescriptions = new List<UrlDescription>
            //    {
            //        new UrlDescription
            //        {
            //            Description = "Орел, 1 комнатные, вторички, не 1 и не последний этаж",
            //            Url = "https://www.cian.ru/cat.php?deal_type=sale&engine_version=2&floornl=1&is_first_floor=0&m2=1&object_type%5B0%5D=1&offer_type=flat&region=175604&room1=1"
            //        },
            //        new UrlDescription
            //        {
            //            Description = "Орел, 3 комнатные, вторички, не 1 и не последний этаж",
            //            Url = "https://www.cian.ru/cat.php?deal_type=sale&engine_version=2&floornl=1&is_first_floor=0&m2=1&object_type%5B0%5D=1&offer_type=flat&region=175604&room3=1"
            //        }
            //    }
            //};

            foreach (var urlDescription in urlDescriptionMap.UrlDescriptions)
            {
                var filePath = "districts.txt";
                File.AppendAllText(filePath, urlDescription.Url);
                File.AppendAllText(filePath, Environment.NewLine);
                File.AppendAllText(filePath, urlDescription.Description);
                File.AppendAllText(filePath, Environment.NewLine);

                var districts = urlDescriptionMap.Districts;

                if (urlDescription.Districts != null && urlDescription.Districts.Count > 0)
                {
                    districts = urlDescription.Districts;
                }

                foreach (var district in districts)
                {

                    driver.Navigate().GoToUrl(urlDescription.Url);

                    if (TrySelectDistrict(driver, district, filePath, true))
                    {
                        _logger.Info($"Пытаюсь найти input для ввода минимальной цены");
                        var minPriceElement = driver.FindElement(By.Name("min"));
                        int totalAdvertCount;
                        var pricePercentile = GetPricePercentile(minPriceElement, driver, district, out totalAdvertCount);
                        var districtInfo = $"АО:{district.Region,-8} Район:{district.Name,-30} Цена за м2:{pricePercentile,-10} Количество объявлений:{totalAdvertCount,-10}";
                        _logger.Info($"Заношу данные {districtInfo}");

                        File.AppendAllText(filePath, districtInfo);
                        File.AppendAllText(filePath, Environment.NewLine);
                    }
                }
            }

            driver.Close();
            Console.ReadLine();
        }

        public static bool TrySelectDistrict(ChromeDriver driver, District district, string filePath, bool hasManyRegions = true)
        {
            _logger.Info("Пытаюсь найти кнопку Район...");
            driver.FindElement(By.XPath("//button[contains(text(), 'Район')]")).Click();
            Thread.Sleep(TimeSpan.FromSeconds(8));

            IWebElement districtElement;
            var actions = new Actions(driver);

            if (hasManyRegions)
            {
                driver.FindElement(By.XPath("//div[contains(text(), 'северо-запад')]")).Click();

                //actions.SendKeys(Keys.ArrowUp);
                //actions.Perform();

                _logger.Info($"Пытаюсь нажать район {district.Name}");
                districtElement = driver.FindElement(By.XPath($"//li/div[text() = '{district.Name}']"));
            }
            else
            {
                _logger.Info($"Пытаюсь нажать район {district.Name}");
                districtElement = driver.FindElement(By.XPath($"//div[text() = '{district.Name}']"));
            }

            //actions.MoveToElement(districtElement);
            //actions.Perform();
            //Thread.Sleep(TimeSpan.FromSeconds(8));
            for (int i = 0; i < district.ArrowDownCount; i++)
            {
                actions.SendKeys(Keys.ArrowDown);
                actions.Perform();
            }
            Thread.Sleep(TimeSpan.FromSeconds(8));
            districtElement.Click();
            Thread.Sleep(TimeSpan.FromSeconds(8));
            _logger.Info($"Пытаюсь нажать кнопку показать * объектов");

            try
            {
                var el = driver.FindElement((By.XPath("//button[contains(text(), ' 0 объектов')]")));
                _logger.Info($"Не найдено объявлений для района {district.Name}");
                File.AppendAllText(filePath, $"АО:{district.Region,-8} Район:{district.Name,-30} Цена за м2:{0,-10} Количество объявлений:{0,-10}");
                File.AppendAllText(filePath, Environment.NewLine);
                return false;
            }
            catch (Exception)
            {

            }

            driver.FindElements((By.XPath("//button[contains(text(), 'Показать')]")))[1].Click();
            Thread.Sleep(TimeSpan.FromSeconds(8));
            return true;
        }

        public static int GetPricePercentile(IWebElement minPriceElement, ChromeDriver driver, District district, out int totalAdvertCount)
        {
            var currentPrice = district.EstimatedPrice;
            var previousPrice = 0;
            var priceOffset = 1000;
            var currentCount = 0;
            var previousCount = 0;
            totalAdvertCount = GetTotalCount(driver);
            if (totalAdvertCount < 1)
            {
                return 0;
            }
            var halfTotalCount = totalAdvertCount / 2;
            _logger.Info($"Общее количество объявлений:{totalAdvertCount} Медианное количество объявлений:{halfTotalCount}");
            var isFirstHalfTotalCountLessCurrentCount = false;
            var isFirstHalfTotalCountMoreCurrentCount = false;
            while (true)
            {
                for (int i = 0; i < 11; i++)
                {
                    minPriceElement.SendKeys(Keys.Backspace);
                }
                minPriceElement.SendKeys(currentPrice.ToString());
                Thread.Sleep(TimeSpan.FromSeconds(8));
                previousCount = currentCount;
                currentCount = GetCount(driver, minPriceElement);
                _logger.Info($"Предыдущая количество объявлений:{previousCount} Текущая количество объявлений:{currentCount}");
                if (halfTotalCount == currentCount)
                {
                    return currentPrice;
                }
                else if (halfTotalCount < currentCount)
                {
                    if (isFirstHalfTotalCountMoreCurrentCount)
                    {
                        return GetWeightAveragePrice(
                            previousPrice: previousPrice,
                            previousCount: previousCount,
                            currentPrice: currentPrice,
                            currentCount: currentCount,
                            halfTotalCount: halfTotalCount);
                    }
                    if (!isFirstHalfTotalCountLessCurrentCount && !isFirstHalfTotalCountMoreCurrentCount)
                    {
                        isFirstHalfTotalCountLessCurrentCount = true;
                    }
                    _logger.Info($"Предыдущая цена:{previousPrice} Текущая цена:{currentPrice}");
                    previousPrice = currentPrice;
                    currentPrice += priceOffset;
                    _logger.Info($"Предыдущая цена:{previousPrice} Текущая цена:{currentPrice}");
                }
                else
                {
                    if (isFirstHalfTotalCountLessCurrentCount)
                    {
                        return GetWeightAveragePrice(
                           previousPrice: previousPrice,
                           previousCount: previousCount,
                           currentPrice: currentPrice,
                           currentCount: currentCount,
                           halfTotalCount: halfTotalCount);
                    }
                    if (!isFirstHalfTotalCountLessCurrentCount && !isFirstHalfTotalCountMoreCurrentCount)
                    {
                        isFirstHalfTotalCountMoreCurrentCount = true;
                    }
                    _logger.Info($"Предыдущая цена:{previousPrice} Текущая цена:{currentPrice}");
                    previousPrice = currentPrice;
                    currentPrice -= priceOffset;
                    _logger.Info($"Предыдущая цена:{previousPrice} Текущая цена:{currentPrice}");
                }
            }
        }

        public static int GetWeightAveragePrice(int previousPrice, int previousCount, int currentPrice, int currentCount, int halfTotalCount)
        {
            return (previousPrice < currentPrice ? previousPrice : currentPrice) +  Math.Abs((previousPrice - currentPrice) / (currentCount - previousCount)) *
                Math.Abs((previousCount < halfTotalCount ? halfTotalCount - previousCount : currentCount - halfTotalCount));
        }

        public static int GetCount(ChromeDriver driver, IWebElement minPriceElement)
        {
            while (true)
            {
                Thread.Sleep(TimeSpan.FromSeconds(8));
                minPriceElement.SendKeys(Keys.Backspace);
                minPriceElement.SendKeys("0");
                Thread.Sleep(TimeSpan.FromSeconds(8));
                //Если объектов 0, то показывается кнопка Найдено 0 объектов
                IWebElement countElement;
                try
                {
                    countElement = driver.FindElement((By.XPath("//button[contains(text(), 'Найдено 0 объектов')]")));
                    return 0;
                }
                catch (Exception)
                {
                }
                
                countElement = driver.FindElement((By.XPath("//button[contains(text(), 'Показать')]")));
                var regex = new Regex(@"Показать (.+?) объект");
                var match = regex.Match(countElement.Text);
                if (match.Success)
                {
                    return int.Parse(match.Groups[1].Value.Replace(" ",""));
                }
            }
        }

        public static int GetTotalCount(ChromeDriver driver)
        {
            Thread.Sleep(TimeSpan.FromSeconds(8));
            var countTotalElement = driver.FindElement((By.XPath("//h3[contains(text(), 'Найдено')]")));
            var regex = new Regex(@"Найдено (.+?) об");
            var match = regex.Match(countTotalElement.Text);
            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value.Replace(" ",""));
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
