# Google Calendar API Documentation

This documentation provides information on how to use the Google Calendar API for adding, deleting and editing events in a calendar.
## Credentials File and Setting up API

Before you can start using the Google Calendar API, you need to create a project on the Google Cloud Platform and enable the Calendar API. 
Once that is done, you need to create a credentials file that will allow your application to access the API. Here are the steps to do that:

1. Go to the Google Cloud Console and create a new project.
2. Enable the Calendar API in the APIs & Services section.
3. Go to the Credentials section and create a new OAuth client ID.
4. Select "Desktop app" as the application type and give your client ID a name.
5. Download the client_secret.json file and save it in your project directory.

## Adding Events

To add an event to a calendar, you first need to authenticate your application using the credentials file you created earlier. 
Once you have authenticated, you can create a new event by sending a POST request to the Calendar API. Here's an example:

```
from google.oauth2 import service_account
from googleapiclient.discovery import build
from datetime import datetime, timedelta

# Authenticate
SCOPES = ['https://www.googleapis.com/auth/calendar']
SERVICE_ACCOUNT_FILE = 'path/to/your/credentials.json'
credentials = service_account.Credentials.from_service_account_file(
    SERVICE_ACCOUNT_FILE, scopes=SCOPES)
service = build('calendar', 'v3', credentials=credentials)

# Create event
event = {
    'summary': 'Test Event',
    'location': 'New York City',
    'description': 'This is a test event.',
    'start': {
        'dateTime': datetime.now().isoformat(),
        'timeZone': 'America/New_York',
    },
    'end': {
        'dateTime': (datetime.now() + timedelta(hours=1)).isoformat(),
        'timeZone': 'America/New_York',
    },
}

# Send request to API
event = service.events().insert(calendarId='primary', body=event).execute()
print(f"Event created: {event.get('htmlLink')}")
```
In this example, we create a new event with a summary, location, description, start time, and end time. 
We then send a POST request to the API to add the event to the primary calendar.

## Deleting Events
To delete an event from a calendar, you need to send a DELETE request to the Calendar API. Here's an example:
```
# Delete event
event_id = 'your-event-id'
service.events().delete(calendarId='primary', eventId=event_id).execute()
print(f"Event deleted: {event_id}")
```
In this example, we specify the ID of the event we want to delete and send a DELETE request to the API.
## Editing Events
To edit an event in a calendar, you need to send a PUT request to the Calendar API with the updated event information. Here's an example:
```
# Update event
event_id = 'your-event-id'
event = service.events().get(calendarId='primary', eventId=event_id).execute()
event['summary'] = 'Updated Test Event'
updated_event = service.events().update(calendarId='primary', eventId=event_id, body=event).execute()
print(f"Event updated: {updated_event.get('htmlLink')}")
```
In this example, we first retrieve the event we want to update using its ID. 
We then update the summary of the event and send a PUT request to the API with the updated event information.

