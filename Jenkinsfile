pipeline {
    agent any

    environment {
        APP_NAME = 'travelfinder-api'
        CONTAINER_NAME = 'travelfinder-api'
        HOST_PORT = '8089'
        CONTAINER_PORT = '8080'
    }

    stages {

        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Restore') {
            steps {
                sh 'dotnet restore TravelFinderApi/TravelFinderApi.csproj'
            }
        }

        stage('Build') {
            steps {
                sh 'dotnet build TravelFinderApi/TravelFinderApi.csproj --configuration Release --no-restore'
            }
        }

        stage('Docker Build') {
            steps {
                sh 'docker build -t ${APP_NAME}:${BUILD_NUMBER} .'
            }
        }

        stage('Deploy') {
            steps {
                sh '''
                    docker stop ${CONTAINER_NAME} || true
                    docker rm ${CONTAINER_NAME} || true

                    docker run -d \
                        --name ${CONTAINER_NAME} \
                        -p ${HOST_PORT}:${CONTAINER_PORT} \
                        ${APP_NAME}:${BUILD_NUMBER}
                '''
            }
        }
    }
}