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
        private static readonly List<District> _districts = new List<District>()
        {
            new District { Name = "Внуковское"                  ,        EstimatedPrice = 170000, Region = "НАО" },
            new District { Name = "Воскресенское"               ,        EstimatedPrice = 142000, Region = "НАО" },
            new District { Name = "Десёновское"                 ,        EstimatedPrice = 142000, Region = "НАО" },
            new District { Name = "Кокошкино"                   ,        EstimatedPrice = 135000, Region = "НАО" },
            new District { Name = "Марушкинское"                ,        EstimatedPrice = 132000, Region = "НАО" },
            new District { Name = "Московский"                  ,        EstimatedPrice = 154000, Region = "НАО" },
            new District { Name = "Мосрентген"                  ,        EstimatedPrice = 166000, Region = "НАО" },
            new District { Name = "Рязановское"                 ,        EstimatedPrice = 138000, Region = "НАО" },
            new District { Name = "Сосенское"                   ,        EstimatedPrice = 160000, Region = "НАО" },
            new District { Name = "Филимонковское"              ,        EstimatedPrice = 116000, Region = "НАО" },
            new District { Name = "Щербинка"                    ,        EstimatedPrice = 140000, Region = "НАО" },
            
            new District { Name = "Куркино"                     ,        EstimatedPrice = 220000, Region = "СЗАО" },
            new District { Name = "Митино"                      ,        EstimatedPrice = 190000, Region = "СЗАО" },
            new District { Name = "Покровское-Стрешнево"        ,        EstimatedPrice = 204000, Region = "СЗАО" },
            new District { Name = "Северное Тушино"             ,        EstimatedPrice = 203000, Region = "СЗАО" },
            new District { Name = "Строгино"                    ,        EstimatedPrice = 237000, Region = "СЗАО" },
            new District { Name = "Хорошево-Мневники"           ,        EstimatedPrice = 251000, Region = "СЗАО" },
            new District { Name = "Щукино"                      ,        EstimatedPrice = 294000, Region = "СЗАО" },
            new District { Name = "Южное Тушино"                ,        EstimatedPrice = 206000, Region = "СЗАО" },
            
            new District { Name = "Аэропорт"                    ,        EstimatedPrice = 298000, Region = "САО" },
            new District { Name = "Беговой"                     ,        EstimatedPrice = 287000, Region = "САО" },
            new District { Name = "Бескудниковский"             ,        EstimatedPrice = 209000, Region = "САО" },
            new District { Name = "Войковский"                  ,        EstimatedPrice = 254000, Region = "САО" },
            new District { Name = "Восточное Дегунино"          ,        EstimatedPrice = 198000, Region = "САО" },
            new District { Name = "Головинский"                 ,        EstimatedPrice = 236000, Region = "САО" },
            new District { Name = "Дмитровский"                 ,        EstimatedPrice = 181000, Region = "САО" },
            new District { Name = "Западное Дегунино"           ,        EstimatedPrice = 198000, Region = "САО" },
            new District { Name = "Коптево"                     ,        EstimatedPrice = 217000, Region = "САО" },
            new District { Name = "Левобережный"                ,        EstimatedPrice = 255000, Region = "САО" },
            new District { Name = "Молжаниновский"              ,        EstimatedPrice = 125000, Region = "САО" },
            new District { Name = "Савеловский"                 ,        EstimatedPrice = 254000, Region = "САО" },
            new District { Name = "Сокол"                       ,        EstimatedPrice = 285000, Region = "САО" },
            new District { Name = "Тимирязевский"               ,        EstimatedPrice = 258000, Region = "САО" },
            new District { Name = "Ховрино"                     ,        EstimatedPrice = 234000, Region = "САО" },
            new District { Name = "Хорошевский"                 ,        EstimatedPrice = 313000, Region = "САО" },
            
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
            
            new District { Name = "Вороновское"                 ,        EstimatedPrice = 139000, Region = "ТАО" },
            new District { Name = "Киевский"                    ,        EstimatedPrice = 109000, Region = "ТАО" },
            new District { Name = "Клёновское"                  ,        EstimatedPrice = 90000, Region = "ТАО" },
            new District { Name = "Краснопахорское"             ,        EstimatedPrice = 149000, Region = "ТАО" },
            new District { Name = "Михайлово-Ярцевское"         ,        EstimatedPrice = 126000, Region = "ТАО" },
            new District { Name = "Новофёдоровское"             ,        EstimatedPrice = 103000, Region = "ТАО" },
            new District { Name = "Первомайское"                ,        EstimatedPrice = 112000, Region = "ТАО" },
            new District { Name = "Роговское"                   ,        EstimatedPrice = 71000, Region = "ТАО" },
            new District { Name = "Троицк"                      ,        EstimatedPrice = 151000, Region = "ТАО" },
            new District { Name = "Щаповское"                   ,        EstimatedPrice = 99000, Region = "ТАО" },
            
            new District { Name = "Внуково"                     ,        EstimatedPrice = 146000, Region = "ЗАО" },
            new District { Name = "Дорогомилово"                ,        EstimatedPrice = 446000, Region = "ЗАО" },
            new District { Name = "Крылатское"                  ,        EstimatedPrice = 234000, Region = "ЗАО" },
            new District { Name = "Кунцево"                     ,        EstimatedPrice = 232000, Region = "ЗАО" },
            new District { Name = "Можайский"                   ,        EstimatedPrice = 221000, Region = "ЗАО" },
            new District { Name = "Ново-Переделкино"            ,        EstimatedPrice = 193000, Region = "ЗАО" },
            new District { Name = "Очаково-Матвеевское"         ,        EstimatedPrice = 258000, Region = "ЗАО" },
            new District { Name = "Проспект Вернадского"        ,        EstimatedPrice = 294000, Region = "ЗАО" },
            new District { Name = "Раменки"                     ,        EstimatedPrice = 326000, Region = "ЗАО" },
            new District { Name = "Солнцево"                    ,        EstimatedPrice = 205000, Region = "ЗАО" },
            new District { Name = "Тропарево-Никулино"          ,        EstimatedPrice = 301000, Region = "ЗАО" },
            new District { Name = "Филевский парк"              ,        EstimatedPrice = 300000, Region = "ЗАО" },
            new District { Name = "Фили-Давыдково"              ,        EstimatedPrice = 264000, Region = "ЗАО" },
            new District { Name = "Конезавод"                   ,        EstimatedPrice = 220000, Region = "ЗАО" },
            new District { Name = "Рублёво-Архангельское"       ,        EstimatedPrice = 220000, Region = "ЗАО" },
            new District { Name = "Сколково"                    ,        EstimatedPrice = 220000, Region = "ЗАО" },
            
            new District { Name = "Арбат"                       ,        EstimatedPrice = 708000, Region = "ЦАО" },
            new District { Name = "Басманный"                   ,        EstimatedPrice = 321000, Region = "ЦАО" },
            new District { Name = "Замоскворечье"               ,        EstimatedPrice = 400000, Region = "ЦАО" },
            new District { Name = "Красносельский"              ,        EstimatedPrice = 369000, Region = "ЦАО" },
            new District { Name = "Мещанский"                   ,        EstimatedPrice = 344000, Region = "ЦАО" },
            new District { Name = "Пресненский"                 ,        EstimatedPrice = 441000, Region = "ЦАО" },
            new District { Name = "Таганский"                   ,        EstimatedPrice = 354000, Region = "ЦАО" },
            new District { Name = "Тверской"                    ,        EstimatedPrice = 510000, Region = "ЦАО" },
            new District { Name = "Хамовники"                   ,        EstimatedPrice = 630000, Region = "ЦАО" },
            new District { Name = "Якиманка"                    ,        EstimatedPrice = 701000, Region = "ЦАО" },
            
            new District { Name = "Богородское"                 ,        EstimatedPrice = 230000, Region = "ВАО" },
            new District { Name = "Вешняки"                     ,        EstimatedPrice = 183000, Region = "ВАО" },
            new District { Name = "Восточное Измайлово"         ,        EstimatedPrice = 195000, Region = "ВАО" },
            new District { Name = "Восточный"                   ,        EstimatedPrice = 151000, Region = "ВАО" },
            new District { Name = "Гольяново"                   ,        EstimatedPrice = 185000, Region = "ВАО" },
            new District { Name = "Ивановское"                  ,        EstimatedPrice = 187000, Region = "ВАО" },
            new District { Name = "Измайлово"                   ,        EstimatedPrice = 215000, Region = "ВАО" },
            new District { Name = "Косино-Ухтомский"            ,        EstimatedPrice = 180000, Region = "ВАО" },
            new District { Name = "Метрогородок"                ,        EstimatedPrice = 201000, Region = "ВАО" },
            new District { Name = "Новогиреево"                 ,        EstimatedPrice = 198000, Region = "ВАО" },
            new District { Name = "Новокосино"                  ,        EstimatedPrice = 179000, Region = "ВАО" },
            new District { Name = "Перово"                      ,        EstimatedPrice = 208000, Region = "ВАО" },
            new District { Name = "Преображенское"              ,        EstimatedPrice = 236000, Region = "ВАО" },
            new District { Name = "Северное Измайлово"          ,        EstimatedPrice = 190000, Region = "ВАО" },
            new District { Name = "Соколиная гора"              ,        EstimatedPrice = 221000, Region = "ВАО" },
            new District { Name = "Сокольники"                  ,        EstimatedPrice = 244000, Region = "ВАО" },
            
            new District { Name = "Академический"               ,        EstimatedPrice = 288000, Region = "ЮЗАО" },
            new District { Name = "Гагаринский"                 ,        EstimatedPrice = 313000, Region = "ЮЗАО" },
            new District { Name = "Зюзино"                      ,        EstimatedPrice = 221000, Region = "ЮЗАО" },
            new District { Name = "Коньково"                    ,        EstimatedPrice = 232000, Region = "ЮЗАО" },
            new District { Name = "Котловка"                    ,        EstimatedPrice = 233000, Region = "ЮЗАО" },
            new District { Name = "Ломоносовский"               ,        EstimatedPrice = 290000, Region = "ЮЗАО" },
            new District { Name = "Обручевский"                 ,        EstimatedPrice = 284000, Region = "ЮЗАО" },
            new District { Name = "Северное Бутово"             ,        EstimatedPrice = 200000, Region = "ЮЗАО" },
            new District { Name = "Теплый Стан"                 ,        EstimatedPrice = 221000, Region = "ЮЗАО" },
            new District { Name = "Черемушки"                   ,        EstimatedPrice = 316000, Region = "ЮЗАО" },
            new District { Name = "Южное Бутово"                ,        EstimatedPrice = 175000, Region = "ЮЗАО" },
            new District { Name = "Ясенево"                     ,        EstimatedPrice = 193000, Region = "ЮЗАО" },
            
            new District { Name = "Бирюлево Восточное"          ,        EstimatedPrice = 154000, Region = "ЮАО" },
            new District { Name = "Бирюлево Западное"           ,        EstimatedPrice = 161000, Region = "ЮАО" },
            new District { Name = "Братеево"                    ,        EstimatedPrice = 186000, Region = "ЮАО" },
            new District { Name = "Даниловский"                 ,        EstimatedPrice = 300000, Region = "ЮАО" },
            new District { Name = "Донской"                     ,        EstimatedPrice = 254000, Region = "ЮАО" },
            new District { Name = "Зябликово"                   ,        EstimatedPrice = 203000, Region = "ЮАО" },
            new District { Name = "Москворечье-Сабурово"        ,        EstimatedPrice = 200000, Region = "ЮАО" },
            new District { Name = "Нагатино-Садовники"          ,        EstimatedPrice = 217000, Region = "ЮАО" },
            new District { Name = "Нагатинский затон"           ,        EstimatedPrice = 251000, Region = "ЮАО" },
            new District { Name = "Нагорный"                    ,        EstimatedPrice = 259000, Region = "ЮАО" },
            new District { Name = "Орехово-Борисово Северное"   ,        EstimatedPrice = 199000, Region = "ЮАО" },
            new District { Name = "Орехово-Борисово Южное"      ,        EstimatedPrice = 203000, Region = "ЮАО" },
            new District { Name = "Царицыно"                    ,        EstimatedPrice = 195000, Region = "ЮАО" },
            new District { Name = "Чертаново Северное"          ,        EstimatedPrice = 226000, Region = "ЮАО" },
            new District { Name = "Чертаново Центральное"       ,        EstimatedPrice = 199000, Region = "ЮАО" },
            new District { Name = "Чертаново Южное"             ,        EstimatedPrice = 204000, Region = "ЮАО" },
            
            new District { Name = "Выхино-Жулебино"             ,        EstimatedPrice = 185000, Region = "ЮВАО" },
            new District { Name = "Капотня"                     ,        EstimatedPrice = 156000, Region = "ЮВАО" },
            new District { Name = "Кузьминки"                   ,        EstimatedPrice = 194000, Region = "ЮВАО" },
            new District { Name = "Лефортово"                   ,        EstimatedPrice = 232000, Region = "ЮВАО" },
            new District { Name = "Люблино"                     ,        EstimatedPrice = 161000, Region = "ЮВАО" },
            new District { Name = "Марьино"                     ,        EstimatedPrice = 190000, Region = "ЮВАО" },
            new District { Name = "Некрасовка"                  ,        EstimatedPrice = 147000, Region = "ЮВАО" },
            new District { Name = "Нижегородский"               ,        EstimatedPrice = 206000, Region = "ЮВАО" },
            new District { Name = "Печатники"                   ,        EstimatedPrice = 188000, Region = "ЮВАО" },
            new District { Name = "Рязанский"                   ,        EstimatedPrice = 226000, Region = "ЮВАО" },
            new District { Name = "Текстильщики"                ,        EstimatedPrice = 166000, Region = "ЮВАО" },
            new District { Name = "Южнопортовый"                ,        EstimatedPrice = 229000, Region = "ЮВАО" },
        };

        static void Main(string[] args)
        {
            var driver = new ChromeDriver(new ChromeOptions() { });
            driver.Manage().Window.Maximize();

            var url = "https://www.cian.ru/cat.php?deal_type=sale&engine_version=2&floornl=1&is_first_floor=0&m2=1&offer_type=flat&region=1&room2=1";
            var filePath = "districts.txt";
            File.AppendAllText(filePath, url);
            File.AppendAllText(filePath, Environment.NewLine);

            foreach (var district in _districts)
            {
                
                driver.Navigate().GoToUrl(url);

                _logger.Info("Пытаюсь найти кнопку Район...");
                driver.FindElement(By.XPath("//button[contains(text(), 'Район')]")).Click();
                Thread.Sleep(TimeSpan.FromSeconds(8));
                driver.FindElement(By.XPath("//div[contains(text(), 'северо-запад')]")).Click();


                Actions actions = new Actions(driver);
                actions.SendKeys(Keys.ArrowUp);
                actions.Perform();

                _logger.Info($"Пытаюсь нажать район {district.Name}");
                var districtElement = driver.FindElement(By.XPath($"//li/div[text() = '{district.Name}']"));
                
                actions.MoveToElement(districtElement);
                actions.Perform();
                Thread.Sleep(TimeSpan.FromSeconds(8));
                for (int i = 0; i < 2; i++)
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
                    File.AppendAllText(filePath, $"АО:{district.Region,-8} Район:{district.Name,-30} Цена за м2:{0,-10} Количество объявлений:{0,-10} Найдно 0 объявлений по данному району");
                    File.AppendAllText(filePath, Environment.NewLine);
                    continue;
                }
                catch (Exception)
                {
                    
                }

                driver.FindElements((By.XPath("//button[contains(text(), 'Показать')]")))[1].Click();
                Thread.Sleep(TimeSpan.FromSeconds(8));
                _logger.Info($"Пытаюсь найти input для ввода минимальной цены");
                var minPriceElement = driver.FindElement(By.Name("min"));
                int totalAdvertCount;
                var pricePercentile = GetPricePercentile(minPriceElement, driver, district, out totalAdvertCount);
                var districtInfo = $"АО:{district.Region, -8} Район:{district.Name,-30} Цена за м2:{pricePercentile,-10} Количество объявлений:{totalAdvertCount,-10}";
                _logger.Info($"Заношу данные {districtInfo}");
                
                File.AppendAllText(filePath, districtInfo);
                File.AppendAllText(filePath, Environment.NewLine);
            }

            //driver.Close();
            Console.ReadLine();
        }

        public static int GetPricePercentile(IWebElement minPriceElement, ChromeDriver driver, District district, out int totalAdvertCount)
        {
            var currentPrice = district.EstimatedPrice;
            var previousPrice = 0;
            var priceOffset = 1000;
            var currentCount = 0;
            var previousCount = 0;
            totalAdvertCount = GetTotalCount(driver);
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
