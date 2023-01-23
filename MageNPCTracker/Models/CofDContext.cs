using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MageNPCTracker.Models
{
    public partial class CofDContext : DbContext
    {
        public CofDContext()
        {
        }

        public CofDContext(DbContextOptions<CofDContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ArcanaTable> ArcanaTable { get; set; }
        public virtual DbSet<ArtifactAttainment> ArtifactAttainment { get; set; }
        public virtual DbSet<ArtifactTable> ArtifactTable { get; set; }
        public virtual DbSet<AttainmentTable> AttainmentTable { get; set; }
        public virtual DbSet<EnhancedTable> EnhancedTable { get; set; }
        public virtual DbSet<ImbuedItemSpell> ImbuedItemSpell { get; set; }
        public virtual DbSet<ImbuedTable> ImbuedTable { get; set; }
        public virtual DbSet<ItemEnhancement> ItemEnhancement { get; set; }
        public virtual DbSet<MageApprenticeTable> MageApprenticeTable { get; set; }
        public virtual DbSet<MageCabal> MageCabal { get; set; }
        public virtual DbSet<MageCaucusInfo> MageCaucusInfo { get; set; }
        public virtual DbSet<MageConsiliumTitles> MageConsiliumTitles { get; set; }
        public virtual DbSet<MageNpcarcana> MageNpcarcana { get; set; }
        public virtual DbSet<MageNpctable> MageNpctable { get; set; }
        public virtual DbSet<MageOrder> MageOrder { get; set; }
        public virtual DbSet<MagePath> MagePath { get; set; }
        public virtual DbSet<Npcartifact> Npcartifact { get; set; }
        public virtual DbSet<Npcgame> Npcgame { get; set; }
        public virtual DbSet<Npcimbued> Npcimbued { get; set; }
        public virtual DbSet<Npctable> Npctable { get; set; }
        public virtual DbSet<RefLegacy> RefLegacy { get; set; }
        public virtual DbSet<RefEnhancement> RefEnhancement { get; set; }
        public virtual DbSet<RefSupernaturalFaction> RefSupernaturalFaction { get; set; }
        public virtual DbSet<Skills> Skills { get; set; }
        public virtual DbSet<SpellArcanaTable> SpellArcanaTable { get; set; }
        public virtual DbSet<SpellTable> SpellTable { get; set; }
        public DbQuery<NPCView> NPCView { get; set; }
        public DbQuery<ImbuedView> ImbuedView { get; set; }
        public DbQuery<ArtifactView> ArtifactView { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=217.180.206.76,49172;Database=CofD;user id=wodadmin;password=serundia!1;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Query<NPCView>().ToView("NPCView");

            modelBuilder.Query<ArtifactView>().ToView("ArtifactView");

            modelBuilder.Query<ImbuedView>().ToView("ImbuedView");

            modelBuilder.Entity<ArcanaTable>(entity =>
            {
                entity.ToTable("Arcana_Table");

                entity.Property(e => e.Arcana)
                    .IsRequired()
                    .HasColumnName("arcana")
                    .HasMaxLength(10);

                entity.Property(e => e.Purview)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<ArtifactAttainment>(entity => {
                entity.ToTable("ArtifactAttainment");

                entity.HasOne(d => d.AtrtifactTable)
                .WithMany(p => p.ArtifactAttainment)
                .HasForeignKey(d => d.ArtifactId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ArtifactAttainment_Artifact_Table");

                entity.HasOne(d => d.AttainmentTable)
                .WithMany(p => p.ArtifactAttainment)
                .HasForeignKey(d => d.AttainmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ArtifactAttainment_Attainment_Table");
            });

            modelBuilder.Entity<ArtifactTable>(entity =>
            {
                entity.ToTable("Artifact_Table");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.ImperialSurcharge);

                entity.Property(e => e.YantraBonus);

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<AttainmentTable>(entity =>
            {
                entity.ToTable("Attainment_Table");

                entity.Property(e => e.attainment_arcana)
                    .HasMaxLength(50);

                entity.Property(e => e.attainment_name)
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<EnhancedTable>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ImbuedTable>(entity =>
            {
                entity.ToTable("Imbued_Table");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BatteryDots).HasColumnName("Battery_Dots");

                entity.Property(e => e.CharacterId).HasColumnName("CharacterID");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.SpellLevel).HasColumnName("Spell_Level");
            });

            modelBuilder.Entity<ImbuedItemSpell>(entity =>
            {
                entity.ToTable("ImbuedItemSpell");

                entity.HasKey(e => e.ImbuedItemSpellId);

                entity.HasOne(d => d.ImbuedTable)
                      .WithMany(p => p.ImbuedItemSpell)
                      .HasForeignKey(d => d.ImbuedItemId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_ImbuedItemSpell_Imbued_Table");

                entity.HasOne(d => d.SpellTable)
                      .WithMany(p => p.ImbuedItemSpell)
                      .HasForeignKey(d => d.SpellId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_ImbuedItemSpell_Spell_Table");
            });

            modelBuilder.Entity<ItemEnhancement>(entity =>
            {
                entity.Property(e => e.Enhancement)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.EnhancedItem)
                    .WithMany(p => p.ItemEnhancement)
                    .HasForeignKey(d => d.EnhancedItemId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ItemEnhancement_EnhancedTable");

                entity.HasOne(d => d.ImbuedItem)
                    .WithMany(p => p.ItemEnhancement)
                    .HasForeignKey(d => d.ImbuedItemId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ItemEnhancement_Imbued_Table");

                entity.HasOne(d => d.Spell)
                    .WithMany(p => p.ItemEnhancement)
                    .HasForeignKey(d => d.SpellId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ItemEnhancement_Spell_Table");

                entity.HasOne(d => d.RefEnhancement)
                    .WithMany(p => p.ItemEnhancement)
                    .HasForeignKey(d => d.EnhancementId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ItemEnhancement_RefEnhancement");
            });

            modelBuilder.Entity<MageApprenticeTable>(entity =>
            {
                entity.HasKey(e => e.ApprenticeNpcid);

                entity.Property(e => e.ApprenticeNpcid).HasColumnName("ApprenticeNPCId");

                entity.Property(e => e.Npcid).HasColumnName("NPCId");

                entity.HasOne(d => d.Mentor)
                    .WithMany(p => p.MageApprenticeTableMentor)
                    .HasForeignKey(d => d.MentorId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_MageApprenticeTable_NPCTable");

                entity.HasOne(d => d.Npc)
                    .WithMany(p => p.MageApprenticeTableNpc)
                    .HasForeignKey(d => d.Npcid)
                    .HasConstraintName("FK_MageApprenticeTable_NPCTable1");
            });

            modelBuilder.Entity<MageCabal>(entity =>
            {
                entity.Property(e => e.CabalName).IsRequired();

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.MageCabal)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_MageCabal_NPCGame");
            });

            modelBuilder.Entity<MageCaucusInfo>(entity =>
            {
                entity.HasKey(e => e.MageCaucusId);

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.MageOrderNavigation)
                    .WithMany(p => p.MageCaucusInfo)
                    .HasForeignKey(d => d.MageOrder)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_MageCaucusInfo_MageOrder");
            });

            modelBuilder.Entity<MageConsiliumTitles>(entity =>
            {
                entity.HasKey(e => e.ConsiliumTitleId);

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MageNpcarcana>(entity =>
            {
                entity.ToTable("MageNPCArcana");

                entity.Property(e => e.MageNpcarcanaId).HasColumnName("MageNPCArcanaId");

                entity.Property(e => e.Npcid).HasColumnName("NPCId");

                entity.HasOne(d => d.Arcana)
                    .WithMany(p => p.MageNpcarcana)
                    .HasForeignKey(d => d.ArcanaId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_MageNPCArcana_Arcana_Table");

                entity.HasOne(d => d.Npc)
                    .WithMany(p => p.MageNpcarcana)
                    .HasForeignKey(d => d.Npcid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_MageNPCArcana_NPCTable");
            });

            modelBuilder.Entity<MageNpctable>(entity =>
            {
                entity.HasKey(e => e.MageNpcid);

                entity.ToTable("MageNPCTable");

                entity.Property(e => e.MageNpcid).HasColumnName("MageNPCId");

                entity.Property(e => e.Npcid).HasColumnName("NPCId");

                entity.Property(e => e.SignatureNimbus).IsRequired();

                entity.HasOne(d => d.Npctable)
                    .WithMany(p => p.MageNpctable)
                    .HasForeignKey(d => d.Npcid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_MageNPCTable_NPCTable");

                entity.HasOne(d => d.ConsiliumStatusNavigation)
                    .WithMany(p => p.MageNpctable)
                    .HasForeignKey(d => d.ConsiliumStatus)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_MageNPCTable_MageConsiliumTitles");

                entity.HasOne(d => d.CabalNavigation)
                    .WithMany(p => p.MageNpctable)
                    .HasForeignKey(d => d.Cabal)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_MageNPCTable_MageCabal");

                entity.HasOne(d => d.LegacyNavigation)
                    .WithMany(p => p.MageNpctable)
                    .HasForeignKey(d => d.Legacy)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_MageNPCTable_RefLegacy");

                entity.HasOne(d => d.OrderNavigation)
                    .WithMany(p => p.MageNpctable)
                    .HasForeignKey(d => d.Order)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_MageNPCTable_MageOrder");

                entity.HasOne(d => d.OrderStatusNavigation)
                    .WithMany(p => p.MageNpctable)
                    .HasForeignKey(d => d.OrderStatus)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_MageNPCTable_MageCaucusInfo");

                entity.HasOne(d => d.PathNavigation)
                    .WithMany(p => p.MageNpctable)
                    .HasForeignKey(d => d.Path)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_MageNPCTable_MagePath");
            });

            modelBuilder.Entity<MageOrder>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.OrderName).IsRequired();
            });

            modelBuilder.Entity<MagePath>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FirstRulingArcana)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SecondRulingArcana)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<Npcartifact>(entity =>
            {
                entity.ToTable("NPCArtifact");

                entity.Property(e => e.NpcartifactId).HasColumnName("NPCArtifactId");

                entity.Property(e => e.Npcid).HasColumnName("NPCId");

                entity.HasOne(d => d.Artifact)
                    .WithMany(p => p.Npcartifact)
                    .HasForeignKey(d => d.ArtifactId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_NPCArtifact_Artifact_Table");

                entity.HasOne(d => d.Npc)
                    .WithMany(p => p.Npcartifact)
                    .HasForeignKey(d => d.Npcid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_NPCArtifact_NPCTable");
            });

            modelBuilder.Entity<Npcgame>(entity =>
            {
                entity.ToTable("NPCGame");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Npcimbued>(entity =>
            {
                entity.ToTable("NPCImbued");

                entity.Property(e => e.NpcimbuedId).HasColumnName("NPCImbuedId");

                entity.Property(e => e.Npcid).HasColumnName("NPCId");

                entity.HasOne(d => d.Imbued)
                    .WithMany(p => p.Npcimbued)
                    .HasForeignKey(d => d.ImbuedId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_NPCImbued_Imbued_Table");

                entity.HasOne(d => d.Npc)
                    .WithMany(p => p.Npcimbued)
                    .HasForeignKey(d => d.Npcid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_NPCImbued_NPCTable");
            });

            modelBuilder.Entity<Npctable>(entity =>
            {
                entity.HasKey(e => e.Npcid);

                entity.ToTable("NPCTable");

                entity.Property(e => e.Npcid).HasColumnName("NPCId");

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(6);

                entity.Property(e => e.RealName).IsRequired();

                entity.HasOne(d => d.SupernaturalFactionNavigation)
                    .WithMany(p => p.Npctable)
                    .HasForeignKey(d => d.SupernaturalFaction)
                    .HasConstraintName("FK_NPCTable_RefSupernaturalFaction");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Npctable)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK_NPCTable_NPCGame");
            });

            modelBuilder.Entity<RefLegacy>(entity =>
            {
                entity.HasKey(e => e.LegacyId);

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.LegacyName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.RefLegacyOrder)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RefLegacy_MageOrder");

                entity.HasOne(d => d.Path)
                    .WithMany(p => p.RefLegacy)
                    .HasForeignKey(d => d.PathId)
                    .HasConstraintName("FK_RefLegacy_MagePath");

                entity.HasOne(d => d.RulingArcanaNavigation)
                    .WithMany(p => p.RefLegacyRulingArcanaNavigation)
                    .HasForeignKey(d => d.RulingArcana)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RefLegacy_Arcana_Table");

                entity.HasOne(d => d.SecondOrder)
                    .WithMany(p => p.RefLegacySecondOrder)
                    .HasForeignKey(d => d.SecondOrderId)
                    .HasConstraintName("FK_RefLegacy_MageOrder1");

                entity.HasOne(d => d.SecondaryArcanaNavigation)
                    .WithMany(p => p.RefLegacySecondaryArcanaNavigation)
                    .HasForeignKey(d => d.SecondaryArcana)
                    .HasConstraintName("FK_RefLegacy_Arcana_Table1");
            });

            modelBuilder.Entity<RefEnhancement>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Bonus)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Cost);
            });

            modelBuilder.Entity<RefSupernaturalFaction>(entity =>
            {
                entity.HasKey(e => e.SupernaturalFactionId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.SupernaturalTrait)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Skills>(entity =>
            {
                entity.HasKey(e => e.SkillId);

                entity.Property(e => e.SkillId).HasColumnName("skillId");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasColumnName("category")
                    .HasMaxLength(10);

                entity.Property(e => e.SkillName)
                    .IsRequired()
                    .HasColumnName("skill_name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SpellArcanaTable>(entity =>
            {
                entity.ToTable("Spell_Arcana_Table");

                entity.Property(e => e.Arcana)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.SpellTableId).HasColumnName("Spell_Table_ID");

                entity.HasOne(d => d.SpellTable)
                    .WithMany(p => p.SpellArcanaTable)
                    .HasForeignKey(d => d.SpellTableId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Spell_Arcana_Table_Spell_Table");
            });

            modelBuilder.Entity<SpellTable>(entity =>
            {
                entity.ToTable("Spell_Table");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.PrimaryFactor).HasMaxLength(10);

                entity.Property(e => e.SpellName)
                    .IsRequired()
                    .HasColumnName("Spell_Name")
                    .HasMaxLength(200);
            });
        }
    }
}
