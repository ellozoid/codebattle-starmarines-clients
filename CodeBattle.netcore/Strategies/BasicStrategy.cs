using System;
using System.Threading.Tasks;
using StarMarines.Models;
using Newtonsoft.Json;
using System.Linq;

namespace StarMarines.Strategies
{
    //Первая конкретная реализация-стратегия 
    public class BasicStrategy : AbstratctStrategy
    {
        public override Command OnReceived(Screen message)
        {
            var command = new Command();
            if (message.Planets.Any())
            {  
                var myPlanets = message.Planets.Where(x => 
                    x.Owner != null && x.Owner.Equals(BotName, StringComparison.InvariantCultureIgnoreCase)).ToList();

                if (myPlanets.Any())
                {
                    myPlanets.ForEach(p => 
                    {
                        var neighbours = p.Neighbours.ToList();
                        var droid = p.Droids / neighbours.Count;
                        neighbours.ForEach(x => 
                        {
                            command.Actions.Add(
                                new Models.Action
                                { 
                                    From = p.Id,
                                    To = x,
                                    UnitsCount = droid
                                }
                            );
                        });
                    });
                }
                else
                {
                    Console.WriteLine("У вас больше нет планет.");
                }
            }
            if (message.Errors.Any())
            {
                Console.WriteLine(JsonConvert.SerializeObject(message.Errors));
            }
            return command;
        }
    }
}