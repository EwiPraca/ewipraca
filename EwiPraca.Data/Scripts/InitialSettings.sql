INSERT INTO [dbo].[Setting]
           ([SettingName]
           ,[SettingDescription]
           ,[SettingValueType])
     VALUES
           ('OSHExpirationNumberOfDaysBeforeWarning'
           ,'Liczba dni przed upływem ważności szkolenia BHP pracownika gdy będzie wysłane powiadomienie mailowe.'
           ,'System.Int32')

INSERT INTO [dbo].[Setting]
           ([SettingName]
           ,[SettingDescription]
           ,[SettingValueType])
     VALUES
           ('MedicalResultNumberOfDaysBeforeWarning'
           ,'Liczba dni przed upływem ważności badania lekarskiego pracownika gdy będzie wysłane powiadomienie mailowe.'
           ,'System.Int32')
