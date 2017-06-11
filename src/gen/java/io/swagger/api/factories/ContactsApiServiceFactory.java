package io.swagger.api.factories;

import io.swagger.api.ContactsApiService;
import io.swagger.api.impl.ContactsApiServiceImpl;

@javax.annotation.Generated(value = "class io.swagger.codegen.languages.JaxRSServerCodegen", date = "2015-12-03T07:51:23.751Z")
public class ContactsApiServiceFactory {

   private final static ContactsApiService service = new ContactsApiServiceImpl();

   public static ContactsApiService getContactsApi()
   {
      return service;
   }
}
