using DevPace.DbConnectionSQLite;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevPace.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly IDbContextFactory<DatabaseConnector> _databaseFactory;

        public CustomersController(ILogger<CustomersController> logger, IDbContextFactory<DatabaseConnector> databaseConnector)
        {
            _logger = logger;
            _databaseFactory = databaseConnector;
        }


        #region Endpoints

        [HttpGet]
        public async Task<IEnumerable<Models.Customer>> Get()
        {
            _logger.LogInformation(GetLogPrefix() + "Called 'get'");

            using DatabaseConnector db = _databaseFactory.CreateDbContext();

            return await db.Customers.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Models.Customer?> Get(int id)
        {
            try
            {
                _logger.LogInformation(GetLogPrefix() + $"Called 'get(id)':: id -> {id}");

                using DatabaseConnector db = _databaseFactory.CreateDbContext();

                return await db.Customers.FirstOrDefaultAsync(el => el.Id == id);
            }
            catch (Exception ex)
            {
                throw new HttpRequestException(ex.ToString());
            }
        }

        [HttpPost]
        public async Task<Models.Customer?> Post([FromBody] Models.Customer value)
        {
            try
            {
                _logger.LogInformation(GetLogPrefix() + $"Called 'post' :: id -> {value.Id}");

                using DatabaseConnector db = _databaseFactory.CreateDbContext();

                CheckCustomerValue(value);

                await db.Customers.AddAsync(value);

                await db.SaveChangesAsync();

                return value;
            }
            catch (Exception ex)
            {
                throw new HttpRequestException(ex.Message);
            }
        }

        [HttpPut()]
        public async Task<Models.Customer?> Put([FromBody] Models.Customer value)
        {
            try
            {
                _logger.LogInformation(GetLogPrefix() + $"Called 'put' :: id -> {value.Id}");

                using DatabaseConnector db = _databaseFactory.CreateDbContext();

                CheckCustomerValue(value);

                db.Update(value);

                await db.SaveChangesAsync();

                return value;
            }
            catch (Exception ex)
            {
                throw new BadHttpRequestException(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            try
            {
                _logger.LogInformation(GetLogPrefix() + $"Called 'delete(id)' :: id -> {id}");

                using DatabaseConnector db = _databaseFactory.CreateDbContext();

                if (db.Customers.Any(el => el.Id == id))
                {
                    db.Customers.Remove(db.Customers.Find(id)!);

                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new HttpRequestException(ex.ToString());
            }
        }

        #endregion


        private void CheckCustomerValue(Models.Customer value)
        {
            Verifications.CustomerVerification verification = new(_databaseFactory, value);
            verification.Verify();

            if (!verification.Verified)
                throw new Exception("Customer failed the verification");
        }

        private static string GetLogPrefix()
        {
            return $"{DateTime.UtcNow} :: ";
        }
    }
}
