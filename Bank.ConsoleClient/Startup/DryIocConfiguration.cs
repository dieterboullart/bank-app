using Bank.BL.Services;
using Bank.BL.Services.Interfaces;
using Bank.Data.Context;
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

        container.Register<IBankService, BankService>(Reuse.Scoped);

        #endregion

        #region Data

        container.Register<IEntityMapper, EntityMapper>(Reuse.Singleton);
        container.Register<IBankAccountRepository, BankAccountRepository>(Reuse.Scoped);
        container.Register<IPersonRepository, PersonRepository>(Reuse.Scoped);
        
        container.Register<BankContext>(Reuse.Scoped);

        #endregion

        #region Shared

        container.Register<IClock, SystemClock>(Reuse.Singleton);
        container.Register<ILogger, ConsoleLogger>(Reuse.Singleton);

        #endregion

        return container;
    }
}