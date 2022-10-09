using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Infrastructure.EventStore.Model.OrderEvents;

namespace Ordering.Infrastructure.EventStore.Configurations;

public class OrderEventConfiguration : IEntityTypeConfiguration<OrderEvent>
{
    public void Configure(EntityTypeBuilder<OrderEvent> builder)
    {
        builder.HasKey(e => new {e.CorrelationId, e.Version});
        builder.HasIndex(e => e.CorrelationId);
        builder.HasIndex(e => e.Version);
        builder.Property(e => e.Data).HasColumnType("jsonb");
    }
}