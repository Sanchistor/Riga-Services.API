pipeline {
    agent any

    environment {
        DOCKER_IMAGE = 'riga-services-app:latest'
        POSTGRES_DB = credentials('database_name')
        POSTGRES_USER = credentials('database-user')
        POSTGRES_PASSWORD = credentials('postgres-password') // Reference Jenkins credentials
        CONNECTION_STRING = "Host=db;Database=${POSTGRES_DB};Username=${POSTGRES_USER};Password=${POSTGRES_PASSWORD};Port=5432;"
    }

    stages {
        stage('Checkout') {
            steps {
                // Pull the latest code from GitHub
                git 'https://github.com/Sanchistor/Riga-Services.API.git'
            }
        }

        stage('Build Docker Image') {
            steps {
                script {
                    // Build the Docker image
                    sh "docker build -t ${DOCKER_IMAGE} ."
                }
            }
        }

        stage('Apply Database Migrations') {
            steps {
                script {
                    // Run migrations
                    sh """
                    docker run --rm \
                      --network app-network \
                      -e ASPNETCORE_ENVIRONMENT=Production \
                      -e CONNECTION_STRING=${CONNECTION_STRING} \
                      ${DOCKER_IMAGE} \
                      dotnet ef database update
                    """
                }
            }
        }

        stage('Deploy to Docker') {
            steps {
                script {
                    // Bring down existing containers if necessary
                    sh "docker-compose down"

                    // Create .env file dynamically
                    writeFile file: '.env', text: """
                    POSTGRES_DB=${POSTGRES_DB}
                    POSTGRES_USER=${POSTGRES_USER}
                    POSTGRES_PASSWORD=${POSTGRES_PASSWORD}
                    CONNECTION_STRING=Host=db;Database=${POSTGRES_DB};Username=${POSTGRES_USER};Password=${POSTGRES_PASSWORD};Port=5432;
                    """

                    // Run Docker Compose
                    sh "docker-compose up -d --build"
                }
            }
        }
    }

    post {
        always {
            script {
                // Clean up Docker images and containers (optional)
                sh "docker system prune -f"
            }
        }
        success {
            script {
                // Notify of successful build (optional)
                echo "Pipeline succeeded!"
            }
        }
        failure {
            script {
                // Notify of failed build (optional)
                echo "Pipeline failed!"
            }
        }
    }
}
