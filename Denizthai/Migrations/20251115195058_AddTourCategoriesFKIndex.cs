using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Denizthai.Migrations
{
    public partial class AddTourCategoriesFKIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // drop old FK/index only if exist
            migrationBuilder.Sql(@"ALTER TABLE ""TourCategories"" DROP CONSTRAINT IF EXISTS ""FK_TourCategories_Categories_CategorieId"";");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ""IX_TourCategories_CategorieId"";");

            // copy data from CategorieId to CategoryId if any left (safe no-op if column doesn't exist)
            migrationBuilder.Sql(@"
        DO $$
        BEGIN
            IF EXISTS (SELECT 1 FROM information_schema.columns 
                       WHERE table_name='TourCategories' AND column_name='CategorieId') THEN
                UPDATE ""TourCategories""
                SET ""CategoryId"" = ""CategorieId""
                WHERE ""CategoryId"" IS NULL AND ""CategorieId"" IS NOT NULL;
            END IF;
        END
        $$;
    ");

            // drop old column if exists
            migrationBuilder.Sql(@"ALTER TABLE ""TourCategories"" DROP COLUMN IF EXISTS ""CategorieId"";");

            // create index on CategoryId if not exists
            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ""IX_TourCategories_CategoryId"" ON ""TourCategories"" (""CategoryId"");");

            // add FK on CategoryId if not exists
            migrationBuilder.Sql(@"
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint c
        JOIN pg_class t ON c.conrelid = t.oid
        JOIN pg_namespace n ON t.relnamespace = n.oid
        WHERE c.conname = 'FK_TourCategories_Categories_CategoryId'
          AND t.relname = 'TourCategories'
    ) THEN
        ALTER TABLE ""TourCategories""
        ADD CONSTRAINT ""FK_TourCategories_Categories_CategoryId""
        FOREIGN KEY (""CategoryId"") REFERENCES ""Categories"" (""Id"") ON DELETE CASCADE;
    END IF;
END
$$;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // remove new FK/index if exist
            migrationBuilder.Sql(@"ALTER TABLE ""TourCategories"" DROP CONSTRAINT IF EXISTS ""FK_TourCategories_Categories_CategoryId"";");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ""IX_TourCategories_CategoryId"";");

            // add CategorieId back if not exists (with default 0 to satisfy NOT NULL)
            migrationBuilder.Sql(@"ALTER TABLE ""TourCategories"" ADD COLUMN IF NOT EXISTS ""CategorieId"" integer NOT NULL DEFAULT 0;");

            // copy values back
            migrationBuilder.Sql(@"
        UPDATE ""TourCategories""
        SET ""CategorieId"" = ""CategoryId""
        WHERE (""CategorieId"" = 0 OR ""CategorieId"" IS NULL) AND ""CategoryId"" IS NOT NULL;
    ");

            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ""IX_TourCategories_CategorieId"" ON ""TourCategories"" (""CategorieId"");");

            migrationBuilder.Sql(@"
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint c
        JOIN pg_class t ON c.conrelid = t.oid
        JOIN pg_namespace n ON t.relnamespace = n.oid
        WHERE c.conname = 'FK_TourCategories_Categories_CategorieId'
          AND t.relname = 'TourCategories'
    ) THEN
        ALTER TABLE ""TourCategories""
        ADD CONSTRAINT ""FK_TourCategories_Categories_CategorieId""
        FOREIGN KEY (""CategorieId"") REFERENCES ""Categories"" (""Id"") ON DELETE CASCADE;
    END IF;
END
$$;");
        }

    }
}
