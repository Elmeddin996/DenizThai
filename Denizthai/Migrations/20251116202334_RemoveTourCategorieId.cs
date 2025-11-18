using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Denizthai.Migrations
{
    public partial class RemoveTourCategorieId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // drop FK if exists
            migrationBuilder.Sql(@"ALTER TABLE ""Tours"" DROP CONSTRAINT IF EXISTS ""FK_Tours_Categories_CategorieId"";");

            // drop index if exists
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ""IX_Tours_CategorieId"";");

            // drop the column if exists
            migrationBuilder.Sql(@"ALTER TABLE ""Tours"" DROP COLUMN IF EXISTS ""CategorieId"";");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // add back column nullable (if needed)
            migrationBuilder.Sql(@"ALTER TABLE ""Tours"" ADD COLUMN IF NOT EXISTS ""CategorieId"" integer;");

            // create index if not exists
            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ""IX_Tours_CategorieId"" ON ""Tours"" (""CategorieId"");");

            // add FK if not exists
            migrationBuilder.Sql(@"
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint c
        JOIN pg_class t ON c.conrelid = t.oid
        JOIN pg_namespace n ON t.relnamespace = n.oid
        WHERE c.conname = 'FK_Tours_Categories_CategorieId'
          AND t.relname = 'Tours'
    ) THEN
        ALTER TABLE ""Tours""
        ADD CONSTRAINT ""FK_Tours_Categories_CategorieId""
        FOREIGN KEY (""CategorieId"") REFERENCES ""Categories"" (""Id"") ON DELETE CASCADE;
    END IF;
END
$$;");
        }

    }
}
