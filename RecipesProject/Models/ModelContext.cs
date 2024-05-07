using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RecipesProject.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aboutcontent> Aboutcontents { get; set; }

    public virtual DbSet<Contactu> Contactus { get; set; }

    public virtual DbSet<Homecontent> Homecontents { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<Recipecategory> Recipecategories { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Soldrecipe> Soldrecipes { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Visacard> Visacards { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("USER ID=C##PR;PASSWORD=Test#321;DATA SOURCE=localhost:1521/xe");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("C##PR")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Aboutcontent>(entity =>
        {
            entity.HasKey(e => e.Aboutcontentid).HasName("SYS_C008498");

            entity.ToTable("ABOUTCONTENT");

            entity.Property(e => e.Aboutcontentid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ABOUTCONTENTID");
            entity.Property(e => e.Paragraph)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("PARAGRAPH");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TITLE");
        });

        modelBuilder.Entity<Contactu>(entity =>
        {
            entity.HasKey(e => e.Contactusid).HasName("SYS_C008500");

            entity.ToTable("CONTACTUS");

            entity.Property(e => e.Contactusid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CONTACTUSID");
            entity.Property(e => e.Message)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("MESSAGE");
            entity.Property(e => e.Phonenumber)
                .HasColumnType("NUMBER")
                .HasColumnName("PHONENUMBER");
            entity.Property(e => e.Senderemail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("SENDEREMAIL");
        });

        modelBuilder.Entity<Homecontent>(entity =>
        {
            entity.HasKey(e => e.Homecontentid).HasName("SYS_C008496");

            entity.ToTable("HOMECONTENT");

            entity.Property(e => e.Homecontentid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("HOMECONTENTID");
            entity.Property(e => e.Paragraph)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("PARAGRAPH");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TITLE");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Paymentid).HasName("SYS_C008517");

            entity.ToTable("PAYMENT");

            entity.Property(e => e.Paymentid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("PAYMENTID");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.Cardid)
                .HasColumnType("NUMBER")
                .HasColumnName("CARDID");
            entity.Property(e => e.Paymentdate)
                .HasColumnType("DATE")
                .HasColumnName("PAYMENTDATE");
            entity.Property(e => e.Recipeid)
                .HasColumnType("NUMBER")
                .HasColumnName("RECIPEID");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Recipe).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Recipeid)
                .HasConstraintName("SYS_C008519");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("SYS_C008518");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Recipeid).HasName("SYS_C008507");

            entity.ToTable("RECIPE");

            entity.Property(e => e.Recipeid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("RECIPEID");
            entity.Property(e => e.Approvalstatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("APPROVALSTATUS");
            entity.Property(e => e.Categoryid)
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORYID");
            entity.Property(e => e.Chefid)
                .HasColumnType("NUMBER")
                .HasColumnName("CHEFID");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Ingredients)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("INGREDIENTS");
            entity.Property(e => e.Instructions)
                .HasMaxLength(700)
                .IsUnicode(false)
                .HasColumnName("INSTRUCTIONS");
            entity.Property(e => e.Price)
                .HasColumnType("NUMBER")
                .HasColumnName("PRICE");
            entity.Property(e => e.Recipename)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("RECIPENAME");

            entity.HasOne(d => d.Category).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.Categoryid)
                .HasConstraintName("SYS_C008509");

            entity.HasOne(d => d.Chef).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.Chefid)
                .HasConstraintName("SYS_C008508");
        });

        modelBuilder.Entity<Recipecategory>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("SYS_C008505");

            entity.ToTable("RECIPECATEGORY");

            entity.Property(e => e.Categoryid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORYID");
            entity.Property(e => e.Categoryname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CATEGORYNAME");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("SYS_C008491");

            entity.ToTable("ROLE");

            entity.Property(e => e.Roleid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Rolename)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ROLENAME");
        });

        modelBuilder.Entity<Soldrecipe>(entity =>
        {
            entity.HasKey(e => e.Soldrecipeid).HasName("SYS_C008511");

            entity.ToTable("SOLDRECIPES");

            entity.Property(e => e.Soldrecipeid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("SOLDRECIPEID");
            entity.Property(e => e.Buyerid)
                .HasColumnType("NUMBER")
                .HasColumnName("BUYERID");
            entity.Property(e => e.Purchasedate)
                .HasColumnType("DATE")
                .HasColumnName("PURCHASEDATE");
            entity.Property(e => e.Recipeid)
                .HasColumnType("NUMBER")
                .HasColumnName("RECIPEID");

            entity.HasOne(d => d.Buyer).WithMany(p => p.Soldrecipes)
                .HasForeignKey(d => d.Buyerid)
                .HasConstraintName("SYS_C008513");

            entity.HasOne(d => d.Recipe).WithMany(p => p.Soldrecipes)
                .HasForeignKey(d => d.Recipeid)
                .HasConstraintName("SYS_C008512");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.Testimonialid).HasName("SYS_C008502");

            entity.ToTable("TESTIMONIAL");

            entity.Property(e => e.Testimonialid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("TESTIMONIALID");
            entity.Property(e => e.Approvalstatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("APPROVALSTATUS");
            entity.Property(e => e.Testimonialtext)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("TESTIMONIALTEXT");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.User).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("SYS_C008503");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("SYS_C008493");

            entity.ToTable("User");

            entity.Property(e => e.Userid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Roleid)
                .HasColumnType("NUMBER")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("USERNAME");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.Roleid)
                .HasConstraintName("SYS_C008494");
        });

        modelBuilder.Entity<Visacard>(entity =>
        {
            entity.HasKey(e => e.Cardid).HasName("SYS_C008515");

            entity.ToTable("VISACARD");

            entity.Property(e => e.Cardid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CARDID");
            entity.Property(e => e.Balance)
                .HasColumnType("NUMBER")
                .HasColumnName("BALANCE");
            entity.Property(e => e.Cardnumber)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("CARDNUMBER");
            entity.Property(e => e.Cvv)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("CVV");
            entity.Property(e => e.Expirydate)
                .HasColumnType("DATE")
                .HasColumnName("EXPIRYDATE");
            entity.Property(e => e.Holdername)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("HOLDERNAME");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
