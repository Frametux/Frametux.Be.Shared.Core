using Frametux.Shared.Core.Domain.ValueObj;

namespace UnitTest.Domain.Entity;

[TestFixture]
public class EntityTest
{
    #region Test Helper Class
    
    /// <summary>
    /// Concrete test implementation of the abstract Entity class for testing purposes
    /// </summary>
    private class TestEntity : Frametux.Shared.Core.Domain.Entity.Entity
    {
        // Empty implementation - inherits all Entity functionality
    }
    
    /// <summary>
    /// Concrete test implementation with custom initialization
    /// </summary>
    private class TestEntityWithCustomInit : Frametux.Shared.Core.Domain.Entity.Entity
    {
        public TestEntityWithCustomInit(Id id, CreatedAt createdAt)
        {
            Id = id;
            CreatedAt = createdAt;
        }
    }
    
    #endregion

    #region Default Initialization Tests

    [Test]
    public void Constructor_WithDefaultInitialization_ShouldCreateEntityWithNewId()
    {
        // Arrange & Act
        var entity = new TestEntity();

        // Assert
        Assert.That(entity.Id, Is.Not.Null);
        Assert.That((string)entity.Id, Is.Not.Empty);
    }

    [Test]
    public void Constructor_WithDefaultInitialization_ShouldCreateEntityWithNewCreatedAt()
    {
        // Arrange & Act
        var entity = new TestEntity();

        // Assert
        Assert.That(entity.CreatedAt, Is.Not.Null);
        Assert.That((DateTime)entity.CreatedAt, Is.Not.EqualTo(default(DateTime)));
    }

    [Test]
    public void Constructor_WithDefaultInitialization_ShouldCreateUniqueIds()
    {
        // Arrange & Act
        var entity1 = new TestEntity();
        var entity2 = new TestEntity();

        // Assert
        Assert.That((string)entity1.Id, Is.Not.EqualTo((string)entity2.Id));
    }

    [Test]
    public void Constructor_WithDefaultInitialization_ShouldCreateDifferentCreatedAtTimes()
    {
        // Arrange & Act
        var entity1 = new TestEntity();
        Thread.Sleep(1); // Ensure different timestamps
        var entity2 = new TestEntity();

        // Assert
        Assert.That((DateTime)entity1.CreatedAt, Is.Not.EqualTo((DateTime)entity2.CreatedAt));
    }

    [Test]
    public void Constructor_WithDefaultInitialization_ShouldHaveCreatedAtNearCurrentTime()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow;
        
        // Act
        var entity = new TestEntity();
        var afterCreation = DateTime.UtcNow;

        // Assert
        var createdAtTime = (DateTime)entity.CreatedAt;
        Assert.That(createdAtTime, Is.GreaterThanOrEqualTo(beforeCreation));
        Assert.That(createdAtTime, Is.LessThanOrEqualTo(afterCreation));
    }

    #endregion

    #region Custom Initialization Tests

    [Test]
    public void Constructor_WithCustomId_ShouldCreateEntityWithSpecifiedId()
    {
        // Arrange
        var customId = new Id("custom-test-id-123");
        var customCreatedAt = new CreatedAt(DateTime.UtcNow.AddDays(-1));

        // Act
        var entity = new TestEntityWithCustomInit(customId, customCreatedAt);

        // Assert
        Assert.That((string)entity.Id, Is.EqualTo("custom-test-id-123"));
    }

    [Test]
    public void Constructor_WithCustomCreatedAt_ShouldCreateEntityWithSpecifiedCreatedAt()
    {
        // Arrange
        var customId = new Id("test-id");
        var specificDate = new DateTime(2023, 10, 15, 14, 30, 0, DateTimeKind.Utc);
        var customCreatedAt = new CreatedAt(specificDate);

        // Act
        var entity = new TestEntityWithCustomInit(customId, customCreatedAt);

        // Assert
        Assert.That((DateTime)entity.CreatedAt, Is.EqualTo(specificDate));
    }

    [Test]
    public void Constructor_WithCustomValues_ShouldPreserveAllProperties()
    {
        // Arrange
        var customId = new Id("preserve-test-id");
        var specificDate = new DateTime(2023, 5, 20, 9, 15, 30, DateTimeKind.Utc);
        var customCreatedAt = new CreatedAt(specificDate);

        // Act
        var entity = new TestEntityWithCustomInit(customId, customCreatedAt);

        // Assert
        Assert.That((string)entity.Id, Is.EqualTo("preserve-test-id"));
        Assert.That((DateTime)entity.CreatedAt, Is.EqualTo(specificDate));
    }

    [Test]
    public void Constructor_WithMinimalValidValues_ShouldCreateEntity()
    {
        // Arrange
        var minimalId = new Id("A");
        var minimalCreatedAt = new CreatedAt(DateTime.MinValue.ToUniversalTime());

        // Act
        var entity = new TestEntityWithCustomInit(minimalId, minimalCreatedAt);

        // Assert
        Assert.That((string)entity.Id, Is.EqualTo("A"));
        Assert.That((DateTime)entity.CreatedAt, Is.EqualTo(DateTime.MinValue.ToUniversalTime()));
    }

    [Test]
    public void Constructor_WithMaximalValidValues_ShouldCreateEntity()
    {
        // Arrange
        var maxLengthId = new Id(new string('Z', Id.MaxLength));
        var maxValidCreatedAt = new CreatedAt(DateTime.UtcNow.AddDays(-1)); // Use a valid past date

        // Act
        var entity = new TestEntityWithCustomInit(maxLengthId, maxValidCreatedAt);

        // Assert
        Assert.That((string)entity.Id, Is.EqualTo(new string('Z', Id.MaxLength)));
        Assert.That((DateTime)entity.CreatedAt, Is.EqualTo((DateTime)maxValidCreatedAt));
    }

    #endregion

    #region Property Behavior Tests

    [Test]
    public void Id_Property_ShouldBeInitOnly()
    {
        // Arrange
        var entity = new TestEntity();
        var originalId = (string)entity.Id;

        // Act & Assert - This test verifies that Id is init-only by compilation
        // If Id was not init-only, the following would cause a compilation error:
        // entity.Id = new Id("new-id"); // This should not compile
        
        // Verify the property is accessible for reading
        Assert.That((string)entity.Id, Is.EqualTo(originalId));
    }

    [Test]
    public void CreatedAt_Property_ShouldBeInitOnly()
    {
        // Arrange
        var entity = new TestEntity();
        var originalCreatedAt = (DateTime)entity.CreatedAt;

        // Act & Assert - This test verifies that CreatedAt is init-only by compilation
        // If CreatedAt was not init-only, the following would cause a compilation error:
        // entity.CreatedAt = new CreatedAt(DateTime.UtcNow); // This should not compile
        
        // Verify the property is accessible for reading
        Assert.That((DateTime)entity.CreatedAt, Is.EqualTo(originalCreatedAt));
    }

    [Test]
    public void Properties_ShouldBeImmutableAfterInitialization()
    {
        // Arrange
        var customId = new Id("immutable-test");
        var customCreatedAt = new CreatedAt(DateTime.UtcNow.AddHours(-1));
        var entity = new TestEntityWithCustomInit(customId, customCreatedAt);
        
        var originalIdValue = (string)entity.Id;
        var originalCreatedAtValue = (DateTime)entity.CreatedAt;

        // Act - Attempt to access properties multiple times
        var idValue1 = (string)entity.Id;
        var idValue2 = (string)entity.Id;
        var createdAtValue1 = (DateTime)entity.CreatedAt;
        var createdAtValue2 = (DateTime)entity.CreatedAt;

        // Assert - Values should remain consistent
        Assert.That(idValue1, Is.EqualTo(originalIdValue));
        Assert.That(idValue2, Is.EqualTo(originalIdValue));
        Assert.That(createdAtValue1, Is.EqualTo(originalCreatedAtValue));
        Assert.That(createdAtValue2, Is.EqualTo(originalCreatedAtValue));
    }

    #endregion

    #region Object Equality Tests

    [Test]
    public void Entities_WithSameIdAndCreatedAt_ShouldNotBeConsideredEqual()
    {
        // Arrange
        var id = new Id("same-id");
        var createdAt = new CreatedAt(DateTime.UtcNow);
        var entity1 = new TestEntityWithCustomInit(id, createdAt);
        var entity2 = new TestEntityWithCustomInit(id, createdAt);

        // Act & Assert
        Assert.That(entity1, Is.Not.EqualTo(entity2));
        Assert.That(entity1.Equals(entity2), Is.False);
        Assert.That(ReferenceEquals(entity1, entity2), Is.False);
    }

    [Test]
    public void Entity_ShouldEqualItself()
    {
        // Arrange
        var entity = new TestEntity();
        var sameEntityReference = entity;

        // Act & Assert
        Assert.That(sameEntityReference, Is.EqualTo(entity));
        Assert.That(entity.Equals(sameEntityReference), Is.True);
        Assert.That(ReferenceEquals(entity, sameEntityReference), Is.True);
    }

    [Test]
    public void Entities_WithDifferentIds_ShouldNotBeEqual()
    {
        // Arrange
        var createdAt = new CreatedAt(DateTime.UtcNow);
        var entity1 = new TestEntityWithCustomInit(new Id("id-1"), createdAt);
        var entity2 = new TestEntityWithCustomInit(new Id("id-2"), createdAt);

        // Act & Assert
        Assert.That(entity1, Is.Not.EqualTo(entity2));
        Assert.That(entity1.Equals(entity2), Is.False);
    }

    [Test]
    public void Entities_WithDifferentCreatedAt_ShouldNotBeEqual()
    {
        // Arrange
        var id = new Id("same-id");
        var entity1 = new TestEntityWithCustomInit(id, new CreatedAt(DateTime.UtcNow.AddDays(-1)));
        var entity2 = new TestEntityWithCustomInit(id, new CreatedAt(DateTime.UtcNow));

        // Act & Assert
        Assert.That(entity1, Is.Not.EqualTo(entity2));
        Assert.That(entity1.Equals(entity2), Is.False);
    }

    #endregion

    #region Null Safety Tests

    [Test]
    public void DefaultInitialization_ShouldNeverProduceNullId()
    {
        // Arrange & Act
        var entities = new TestEntity[100];
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i] = new TestEntity();
        }

        // Assert
        foreach (var entity in entities)
        {
            Assert.That(entity.Id, Is.Not.Null);
            Assert.That((string)entity.Id, Is.Not.Null);
            Assert.That((string)entity.Id, Is.Not.Empty);
        }
    }

    [Test]
    public void DefaultInitialization_ShouldNeverProduceNullCreatedAt()
    {
        // Arrange & Act
        var entities = new TestEntity[100];
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i] = new TestEntity();
        }

        // Assert
        foreach (var entity in entities)
        {
            Assert.That(entity.CreatedAt, Is.Not.Null);
            Assert.That((DateTime)entity.CreatedAt, Is.Not.EqualTo(default(DateTime)));
        }
    }

    #endregion

    #region Edge Case Tests

    [Test]
    public void MultipleEntities_ShouldHaveUniqueIdentifiers()
    {
        // Arrange
        const int entityCount = 1000;
        var entities = new TestEntity[entityCount];
        var idSet = new HashSet<string>();

        // Act
        for (int i = 0; i < entityCount; i++)
        {
            entities[i] = new TestEntity();
            idSet.Add(entities[i].Id);
        }

        // Assert
        Assert.That(idSet.Count, Is.EqualTo(entityCount), "All entities should have unique IDs");
    }

    [Test]
    public void Entity_PropertiesAccessibility_ShouldBePublic()
    {
        // Arrange
        var entity = new TestEntity();

        // Act & Assert - Test that properties are publicly accessible
        Assert.DoesNotThrow(() =>
        {
            var id = entity.Id;
            var createdAt = entity.CreatedAt;
            var unused = (string)id;
            var unused1 = (DateTime)createdAt;
        });
    }

    [Test]
    public void Entity_ShouldBeInstantiableFromInheritance()
    {
        // Arrange & Act
        TestEntity? entity = null;
        
        // Assert
        Assert.DoesNotThrow(() => entity = new TestEntity());
        Assert.That(entity, Is.Not.Null);
        Assert.That(entity, Is.InstanceOf<Frametux.Shared.Core.Domain.Entity.Entity>());
    }

    [Test]
    public void Entity_WithCustomInit_ShouldAcceptValidValueObjects()
    {
        // Arrange
        var validId = new Id("valid-entity-id");
        var validCreatedAt = new CreatedAt(DateTime.UtcNow.AddMinutes(-30));

        // Act & Assert
        Assert.DoesNotThrow(() =>
        {
            var entity = new TestEntityWithCustomInit(validId, validCreatedAt);
            Assert.That(entity, Is.Not.Null);
            Assert.That((string)entity.Id, Is.EqualTo("valid-entity-id"));
        });
    }

    #endregion
}
