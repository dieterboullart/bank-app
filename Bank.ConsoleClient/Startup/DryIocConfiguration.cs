using Bank.BL.Services;
using Bank.BL.Services.Interfaces;
using Bank.Data.Mappers;
using Bank.Data.Mappers.Interfaces;
using Bank.Data.Repositories;
using Bank.Data.Repositories.Interfaces;
using Bank.Shared.Logging;
using Bank.Shared.Logging.Interfaces;
using Bank.Shared.Utils.Clock;
using Bank.Shared.Utils.Clock.Interfaces;
using DryIoc;

namespace Bank.ConsoleClient.Startup;

public static class DryIocConfiguration
{
    public static IContainer ConfigureIoc()
    {
        var container = new Container();
        
        #region BL
        
        container.Register<IBankService, BankService>();
        
        #endregion

        #region Data

        container.Register<IEntityMapper, EntityMapper>();
        container.Register<IBankAccountRepository, BankAccountRepository>();
        container.Register<IPersonRepository, PersonRepository>();

        #endregion

        #region Shared

        container.Register<IClock, SystemClock>();
        container.Register<ILogger, ConsoleLogger>();

        #endregion

        return container;
    }
}