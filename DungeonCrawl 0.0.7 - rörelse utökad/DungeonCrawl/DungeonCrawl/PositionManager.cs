
namespace DungeonCrawl
{
    struct PositionManager
    {
        //Klass som skapas till en array. Dessa variabler används för att tilldela vissa värden till vissa positioner i rutnätet.

        public string type; //typ av objekt
        public int iteration; //iterationsnummer för objektet
        public int hp; //hälsa av objekt
        public bool floor;
        public bool entry;
        public string monster;  //Håller koll på vilken typ av monster som befinner sig på positionen
        //Lägg till fler värden efter behov


    }
}
