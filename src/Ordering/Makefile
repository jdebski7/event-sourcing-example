DOTNET_ENVIRONMENT ?= Development
MIGRATOR_STARTUP_PROJECT ?= Ordering.Migrator
MIGRATIONS_PROJECT ?= Ordering.Migrator
SERVICE_TEST_DIR ?= Ordering.Tests

# ------------------------------------------------ DATABASE --------------------------------------------------------
regenerate-initial-migration:
	cd ${MIGRATIONS_PROJECT} && \
	rm -rf Migrations && \
    DOTNET_ENVIRONMENT=${DOTNET_ENVIRONMENT} MIGRATIONS_ASSEMBLY=${MIGRATIONS_PROJECT} dotnet ef migrations add InitialMigration \
    	--context EventStoreDbContext \
		--startup-project ./${MIGRATOR_STARTUP_PROJECT}.csproj \
    	--output-dir Migrations

add-new-migration:
	echo "Migration name: " && read MIGRATION_NAME && \
	cd ${MIGRATIONS_PROJECT} && \
	DOTNET_ENVIRONMENT=${DOTNET_ENVIRONMENT} MIGRATIONS_ASSEMBLY=${MIGRATIONS_PROJECT} MIGRATIONS_GENERATION=true dotnet ef migrations add $$MIGRATION_NAME \
		--context EventStoreDbContext \
		--startup-project ../${MIGRATOR_STARTUP_PROJECT}/${MIGRATOR_STARTUP_PROJECT}.csproj	\
		--output-dir Migrations

remove-last-migration:
	cd ${MIGRATIONS_PROJECT} && \
	DOTNET_ENVIRONMENT=${DOTNET_ENVIRONMENT} MIGRATIONS_ASSEMBLY=${MIGRATIONS_PROJECT} MIGRATIONS_GENERATION=true dotnet ef migrations remove \
		--context EventStoreDbContext \
		--startup-project ./${MIGRATOR_STARTUP_PROJECT}.csproj

drop-database:
	cd ${MIGRATOR_STARTUP_PROJECT} && DOTNET_ENVIRONMENT=${DOTNET_ENVIRONMENT} dotnet run migrator drop-database

migrate-latest:
	cd ${MIGRATOR_STARTUP_PROJECT} && DOTNET_ENVIRONMENT=${DOTNET_ENVIRONMENT} MIGRATIONS_ASSEMBLY=${MIGRATIONS_PROJECT} dotnet run migrator migrate-latest
