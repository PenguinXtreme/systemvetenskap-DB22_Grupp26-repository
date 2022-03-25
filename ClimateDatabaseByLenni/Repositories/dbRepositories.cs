using System;
using System.Collections.Generic;
using System.Text;
using ClimateDatabaseByLenni.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
namespace ClimateDatabaseByLenni.Repositories
{
    class dbRepositories
    {
        private string _connectionString;
        public dbRepositories()
        {
            var config = new ConfigurationBuilder().AddUserSecrets<dbRepositories>()
                .Build();
            _connectionString = config.GetConnectionString("dbConn");
        }
        public List<Person> AddObservers(List<Person> observers)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            using var transaction = conn.BeginTransaction();
            StringBuilder sql = new StringBuilder("insert into observer ");
            sql.AppendLine("(firstname, lastname) ");
            sql.AppendLine("values(@firstname, @lastname) ");

            using var command = new NpgsqlCommand(sql.ToString(), conn);
            foreach (var observer in observers)
            {
                command.Parameters.AddWithValue("firstname", observer.Firstname);
                command.Parameters.AddWithValue("Lastname", observer.Lastname);
                var result = command.ExecuteScalar();
                command.Parameters.Clear();


            }
            transaction.Commit();

            return observers;

        }
        public List<Person> GetClimateObservers()
        {
            var observers = new List<Person>();
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();



            using var cmd = new NpgsqlCommand();
            cmd.CommandText = "select firstname, lastname, id from observer order by lastname";
            cmd.Connection = conn;
            Person observer = null;
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {

                    observer = new Person()
                    {
                        Firstname = (string)reader["firstname"],
                        Lastname = (string)reader["lastname"],
                        Id = (int)reader["id"]
                    };
                    observers.Add(observer);


                }
            }

            return observers;

        }
        public string DeleteChosenObserver(Person observer)
        {
            

            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            
            
            
            
           
           
            StringBuilder sql = new StringBuilder("select ");
            sql.AppendLine("observer_id ");
            sql.AppendLine("from observer ");
            sql.AppendLine("join observation ");
            sql.AppendLine("on observer.id = observer_id ");
            sql.AppendLine("where firstname = @firstname and lastname = @lastname ");

            using var command = new NpgsqlCommand(sql.ToString(), conn);
           
            command.Parameters.AddWithValue("firstname", observer.Firstname);
            command.Parameters.AddWithValue("Lastname", observer.Lastname);
            var result = command.ExecuteReader();
            if (result == null )
            {


                using var cmd = new NpgsqlCommand();
                cmd.CommandText = "Delete from observer where firstname = @firstname and lastname = @lastname ";
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("firstname", observer.Firstname);
                cmd.Parameters.AddWithValue("Lastname", observer.Lastname);
                cmd.ExecuteScalar();


                return $"{observer.Firstname} {observer.Lastname} har blivit raderad från databasen";

            }
            else
            {
                return $" Observatören {observer.Firstname} {observer.Lastname} har gjort observationer, om du tar bort hen vet man inte vem som har redovisat observationerna!";
            }
           
     

        }
        public void AddObserversPlaceAndTemp(string firstname, string lastname, string temperature, int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("insert into observervation  ");
            sql.AppendLine("(observer_id, geolocation) ");
            sql.AppendLine("values(@id, 3); ");
            sql.AppendLine("insert into unit");
            sql.AppendLine("(type, abbreviation) ");
            sql.AppendLine("values (lufttemperatur, @temperatur); ");
            sql.AppendLine("insert into category ");
            sql.AppendLine("(name) ");
            sql.AppendLine("Values (väder) ");
            
            

            using var command = new NpgsqlCommand(sql.ToString(), conn);
            
               
                command.Parameters.AddWithValue("id", id);
                command.Parameters.AddWithValue("temperatur", temperature);
            var result = command.ExecuteScalar();
                                              

        }
        public List<Person> GetTheObservations(int nameId)
        {
            
            var observations = new List<Person>();
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();



            using var cmd = new NpgsqlCommand();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select observation.id  ");
            sql.AppendLine("from observer ");
            sql.AppendLine("inner join observation ");
            sql.AppendLine("on observer.id = observation.observer_id ");
            sql.AppendLine("where observer.id = @id ");
            

            using var command = new NpgsqlCommand(sql.ToString(), conn);

            command.Parameters.AddWithValue("id", nameId);
            
            Person observation = null;

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {

                    observation = new Person()
                    {
                       
                        ObservationId = (int)reader["id"]
                    };
                    observations.Add(observation);


                }
            }

            return observations;



        }
        public List<Person> GetDifferentType(int observationId)
        {

            var observations = new List<Person>();
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();



            using var cmd = new NpgsqlCommand();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select unit.type ");
            sql.AppendLine("from unit ");
            sql.AppendLine("inner join category ");
            sql.AppendLine("on category.unit_id = unit.id ");
            sql.AppendLine("inner join measurement");
            sql.AppendLine("on category.id = measurement.category_id");
            sql.AppendLine("where measurement.observation_id = @id");


            using var command = new NpgsqlCommand(sql.ToString(), conn);

            command.Parameters.AddWithValue("id", observationId);

            Person observation = null;

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {

                    observation = new Person()
                    {

                        Firstname = (string)reader["type"]
                    };
                    observations.Add(observation);


                }
            }

            return observations;
            



        }
        public List<Person> GetAbbreviationMeauserements(int observationId)
        {

            var observations = new List<Person>();
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();



            using var cmd = new NpgsqlCommand();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select unit.abbreviation, unit.id ");
            sql.AppendLine("from unit ");
            sql.AppendLine("inner join category ");
            sql.AppendLine("on category.unit_id = unit.id ");
            sql.AppendLine("inner join measurement");
            sql.AppendLine("on category.id = measurement.category_id");
            sql.AppendLine("where measurement.observation_id = @id");


            using var command = new NpgsqlCommand(sql.ToString(), conn);

            command.Parameters.AddWithValue("id", observationId);

            Person observation = null;

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {

                    observation = new Person()
                    {

                        Firstname = (string)reader["abbreviation"],
                        Id = (int)reader["id"]
                    };
                    observations.Add(observation);


                }
            }

            return observations;

        }
        public void ChangeTheValue(string abbreviationMeasurementchange, int unitId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
           
            StringBuilder sql = new StringBuilder("update unit ");
            sql.AppendLine("Set abbreviation = @abbreviation  ");
            sql.AppendLine("where id = @id ");

            using var command = new NpgsqlCommand(sql.ToString(), conn);
            
            
                command.Parameters.AddWithValue("abbreviation", abbreviationMeasurementchange);
            command.Parameters.AddWithValue("id", unitId);
            var result = command.ExecuteScalar();
                


            
            

            





        }
    }
}     
