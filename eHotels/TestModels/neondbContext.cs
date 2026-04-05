using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace eHotels.TestModels;

public partial class neondbContext : DbContext
{
    public neondbContext()
    {
    }

    public neondbContext(DbContextOptions<neondbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<account> account { get; set; }

    public virtual DbSet<address> address { get; set; }

    public virtual DbSet<booking> booking { get; set; }

    public virtual DbSet<customer> customer { get; set; }

    public virtual DbSet<employee> employee { get; set; }

    public virtual DbSet<hotel> hotel { get; set; }

    public virtual DbSet<hotelamenity> hotelamenity { get; set; }

    public virtual DbSet<hotelchain> hotelchain { get; set; }

    public virtual DbSet<hotelchainemail> hotelchainemail { get; set; }

    public virtual DbSet<hotelchainphone> hotelchainphone { get; set; }

    public virtual DbSet<hotelemail> hotelemail { get; set; }

    public virtual DbSet<hotelimage> hotelimage { get; set; }

    public virtual DbSet<hotelphone> hotelphone { get; set; }

    public virtual DbSet<renting> renting { get; set; }

    public virtual DbSet<review> review { get; set; }

    public virtual DbSet<room> room { get; set; }

    public virtual DbSet<roomamenity> roomamenity { get; set; }

    public virtual DbSet<roomnum> roomnum { get; set; }

    public virtual DbSet<roomnumcity> roomnumcity { get; set; }

    public virtual DbSet<roomproblem> roomproblem { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=ep-sweet-glitter-a8ag8fj1-pooler.eastus2.azure.neon.tech;Database=neondb;Username=neondb_owner;Password=npg_cU7jafXmtI5k;SSLMode=VerifyFull;Channel Binding=Require;Include Error Detail=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<account>(entity =>
        {
            entity.HasKey(e => e.email).HasName("account_pkey");
        });

        modelBuilder.Entity<address>(entity =>
        {
            entity.HasKey(e => e.postalcode).HasName("address_pkey");
        });

        modelBuilder.Entity<booking>(entity =>
        {
            entity.HasKey(e => e.bookingid).HasName("booking_pkey");

            entity.Property(e => e.bookingid).ValueGeneratedNever();

            entity.HasOne(d => d.customer).WithMany(p => p.booking)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("booking_idtype_idnumber_fkey");

            entity.HasOne(d => d.room).WithMany(p => p.booking)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("booking_roomnumber_hotelid_fkey");
        });

        modelBuilder.Entity<customer>(entity =>
        {
            entity.HasKey(e => new { e.idtype, e.idnumber }).HasName("customer_pkey");

            entity.HasOne(d => d.postalcodeNavigation).WithMany(p => p.customer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customer_postalcode_fkey");
        });

        modelBuilder.Entity<employee>(entity =>
        {
            entity.HasKey(e => e.ssn).HasName("employee_pkey");

            entity.HasOne(d => d.emailNavigation).WithMany(p => p.employee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("employee_email_fkey");

            entity.HasOne(d => d.hotel).WithMany(p => p.employee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_employee_hotel");
        });

        modelBuilder.Entity<hotel>(entity =>
        {
            entity.HasKey(e => e.hotelid).HasName("hotel_pkey");

            entity.Property(e => e.hotelid).ValueGeneratedNever();

            entity.HasOne(d => d.chain).WithMany(p => p.hotel).HasConstraintName("hotel_chainid_fkey");

            entity.HasOne(d => d.managerNavigation).WithMany(p => p.hotelNavigation).HasConstraintName("fk_hotel_manager");
        });

        modelBuilder.Entity<hotelamenity>(entity =>
        {
            entity.HasKey(e => new { e.hotelid, e.amenityname }).HasName("hotelamenity_pkey");

            entity.HasOne(d => d.hotel).WithMany(p => p.hotelamenity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hotelamenity_hotelid_fkey");
        });

        modelBuilder.Entity<hotelchain>(entity =>
        {
            entity.HasKey(e => e.chainid).HasName("hotelchain_pkey");

            entity.Property(e => e.chainid).ValueGeneratedNever();
        });

        modelBuilder.Entity<hotelchainemail>(entity =>
        {
            entity.HasKey(e => new { e.chainid, e.email }).HasName("hotelchainemail_pkey");

            entity.HasOne(d => d.chain).WithMany(p => p.hotelchainemail)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hotelchainemail_chainid_fkey");
        });

        modelBuilder.Entity<hotelchainphone>(entity =>
        {
            entity.HasKey(e => new { e.chainid, e.phonenumber }).HasName("hotelchainphone_pkey");

            entity.HasOne(d => d.chain).WithMany(p => p.hotelchainphone)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hotelchainphone_chainid_fkey");
        });

        modelBuilder.Entity<hotelemail>(entity =>
        {
            entity.HasKey(e => new { e.hotelid, e.email }).HasName("hotelemail_pkey");

            entity.HasOne(d => d.hotel).WithMany(p => p.hotelemail)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hotelemail_hotelid_fkey");
        });

        modelBuilder.Entity<hotelimage>(entity =>
        {
            entity.HasKey(e => new { e.hotelid, e.filename }).HasName("hotelimage_pkey");

            entity.HasOne(d => d.hotel).WithMany(p => p.hotelimage)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hotelimage_hotelid_fkey");
        });

        modelBuilder.Entity<hotelphone>(entity =>
        {
            entity.HasKey(e => new { e.hotelid, e.phonenumber }).HasName("hotelphone_pkey");

            entity.HasOne(d => d.hotel).WithMany(p => p.hotelphone)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hotelphone_hotelid_fkey");
        });

        modelBuilder.Entity<renting>(entity =>
        {
            entity.HasKey(e => e.rentingid).HasName("renting_pkey");

            entity.Property(e => e.rentingid).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.customer).WithMany(p => p.renting)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("renting_idtype_idnumber_fkey");

            entity.HasOne(d => d.room).WithMany(p => p.renting)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("renting_roomnumber_hotelid_fkey");
        });

        modelBuilder.Entity<review>(entity =>
        {
            entity.HasKey(e => new { e.email, e.hotelid }).HasName("review_pkey");

            entity.HasOne(d => d.hotel).WithMany(p => p.review)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("review_hotelid_fkey");
        });

        modelBuilder.Entity<room>(entity =>
        {
            entity.HasKey(e => new { e.roomnumber, e.hotelid }).HasName("room_pkey");

            entity.HasOne(d => d.hotel).WithMany(p => p.room)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("room_hotelid_fkey");
        });

        modelBuilder.Entity<roomamenity>(entity =>
        {
            entity.HasKey(e => new { e.roomnumber, e.hotelid, e.amenity }).HasName("roomamenity_pkey");

            entity.HasOne(d => d.room).WithMany(p => p.roomamenity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("roomamenity_roomnumber_hotelid_fkey");
        });

        modelBuilder.Entity<roomnum>(entity =>
        {
            entity.ToView("roomnum");
        });

        modelBuilder.Entity<roomnumcity>(entity =>
        {
            entity.ToView("roomnumcity");
        });

        modelBuilder.Entity<roomproblem>(entity =>
        {
            entity.HasKey(e => new { e.roomnumber, e.hotelid, e.problem }).HasName("roomproblem_pkey");

            entity.HasOne(d => d.room).WithMany(p => p.roomproblem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("roomproblem_roomnumber_hotelid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
