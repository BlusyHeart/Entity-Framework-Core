namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto;
    using Boardgames.Utilities;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            XmlHelper xmlHelper = new XmlHelper();
            StringBuilder sb = new StringBuilder();

            var importCreators = xmlHelper.Deserialize<ImportCreatorsDTO[]>(xmlString, "Creators");

            ICollection<Creator> validCreators= new HashSet<Creator>();

            foreach (var c in importCreators)
            {
                if (!IsValid(c))
                {
                    sb.AppendLine(ErrorMessage); continue;
                }

                ICollection<Boardgame> boardgames = new HashSet<Boardgame>();

                foreach (var b in c.Boardgames)
                {
                    if (!IsValid(b))
                    {
                        sb.AppendLine(ErrorMessage); continue;
                    }

                    Boardgame boardgame = new Boardgame()
                    {
                        Name = b.Name,
                        Rating = b.Rating,
                        YearPublished = b.YearPublished,
                        CategoryType = (CategoryType)b.CategoryType,
                        Mechanics = b.Mechanics
                    };

                    boardgames.Add(boardgame);
                }

                Creator creator = new Creator()
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Boardgames = boardgames                    
                };

                validCreators.Add(creator);
                sb.AppendLine($"Successfully imported creator – {c.FirstName} {c.LastName} with {creator.Boardgames.Count} boardgames.");
            }

            context.AddRange(validCreators);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var importDTOs = JsonConvert.DeserializeObject<ImportSellersDTO[]>(jsonString);

            ICollection<Seller> validSellers = new HashSet<Seller>();
            ICollection<int> validBoardgamesIds = new HashSet<int>();

            foreach (var dto in importDTOs)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Seller seller = new Seller()
                {
                    Name = dto.Name,
                    Address = dto.Address,
                    Country = dto.Country,
                    Website = dto.Website,
                };

                validBoardgamesIds = context.Boardgames.Select(b => b.Id).ToArray();

                foreach (var id in dto.Boardgames.Distinct())
                {
                    if (!validBoardgamesIds.Contains(id))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    BoardgameSeller boardgameSeller = new BoardgameSeller()
                    {
                        Seller = seller,
                        BoardgameId = id
                    };

                    seller.BoardgamesSellers.Add(boardgameSeller);
                   
                }

                validSellers.Add(seller);
                sb.AppendLine($"Successfully imported seller - {seller.Name} with {seller.BoardgamesSellers.Count} boardgames.");

            }

            context.Sellers.AddRange(validSellers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
